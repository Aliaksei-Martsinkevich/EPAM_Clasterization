Option Strict On
Option Explicit On

' ReSharper disable once InconsistentNaming
Public Class LZWCompressor
    ' see http://marknelson.us/1989/10/01/lzw-data-compression/
    ' this is a VB.NET conversion port of mark's C program.
    ' Please refer to that program prior to modifying this one.

    Private ReadOnly _bits As Integer = 14
    Private ReadOnly _hashingShift As Integer = 4
    Private ReadOnly _maxValue As Integer = (1 << _bits) - 1
    Private ReadOnly _maxCode As Integer = _maxValue - 1
    'Private Const TABLE_SIZE As Integer = 5021 ' 12 bits
    'Private Const TABLE_SIZE As Integer = 9029 ' 13 bits
    Private Const TableSize As Integer = 18041 ' 14 bits
    Private _eof As Integer = -1

    Public BrInput As IO.BinaryReader = Nothing
    Public BwOutput As IO.BinaryWriter = Nothing

    Private ReadOnly _iaCodeValue(TableSize) As Integer
    Private ReadOnly _iaPrefixCode(TableSize) As Integer
    Private ReadOnly _baAppendCharacter(TableSize) As Byte

    '** This is the compression routine.  The code should be a fairly close
    '** match to the algorithm accompanying the article.
    Public Sub Compress()
        Dim iNextCode As Integer = 0
        Dim iCharacter As Integer = 0
        Dim iStringCode As Integer = 0
        Dim iIndex As Integer = 0

        iNextCode = 256                   ' Next code is the next available string code

        For i As Integer = 0 To TableSize - 1 ' Clear out the string table before starting
            _iaCodeValue(i) = -1
        Next i

        ' Get the first iCharacter. Assuming it to be 0 - 255
        ' Hence only valid for ASCII text files */
        iStringCode = ReadByte()

        '** This is the main loop where it all happens.  This loop runs util all of
        '** the Input has been exhausted.  Note that it stops adding codes to the
        '** table after all of the possible codes have been defined.
        iCharacter = ReadByte()
        While iCharacter <> -1
            iIndex = find_match(iStringCode, iCharacter)    ' See if the string is in */
            If (_iaCodeValue(iIndex) <> -1) Then            ' the table.  If it is,   */
                iStringCode = _iaCodeValue(iIndex)          ' get the code value.  If */
            Else                                            ' the string is not in the table, try to add it.   */
                If (iNextCode <= _maxCode) Then
                    _iaCodeValue(iIndex) = iNextCode
                    iNextCode += 1
                    _iaPrefixCode(iIndex) = iStringCode
                    _baAppendCharacter(iIndex) = CByte(iCharacter)
                End If
                output_code(iStringCode)    ' When a string is found  */
                iStringCode = iCharacter    ' that is not in the table */
            End If                          ' after adding the new one */
            iCharacter = ReadByte()
        End While

        ' End of the main loop.

        output_code(iStringCode)   ' Output the last code               
        output_code(_maxValue)     ' Output the end of buffer code      */
        output_code(0)             ' This code flushes the Output buffer*/
    End Sub

    ' This is the hashing routine.  It tries to find a match for the prefix+char
    ' string in the string table.  If it finds it, the iIndex is returned.  If
    ' the string is not found, the first available iIndex in the string table is
    ' returned instead.
    Private Function find_match(ByVal iHashPrefix As Integer, ByVal iHashCharacter As Integer) As Integer
        Dim iIndex As Integer = 0
        Dim iOffset As Integer = 0

        iIndex = CInt((iHashCharacter << _hashingShift) Xor iHashPrefix)

        If (iIndex = 0) Then
            iOffset = 1
        Else
            iOffset = TableSize - iIndex
        End If

        While (True)
            If _iaCodeValue(iIndex) = -1 Then
                Return iIndex
            End If
            If (_iaPrefixCode(iIndex) = iHashPrefix) And (_baAppendCharacter(iIndex) = iHashCharacter) Then
                Return iIndex
            End If
            iIndex -= iOffset
            If (iIndex < 0) Then
                iIndex += TableSize
            End If
        End While
    End Function

    ' The following routine is used to output variable length
    ' codes.  It is written strictly for clarity, and is not
    ' particularly efficient.

    Private Sub output_code(ByVal code As Integer)
        Static outputBitCount As Integer = 0
        Static outputBitBuffer As Long = 0

        outputBitBuffer = outputBitBuffer Or (code << (32 - _bits - outputBitCount))
        outputBitCount += _bits

        While outputBitCount >= 8
            WriteByte(CByte((outputBitBuffer >> 24) And 255))
            outputBitBuffer <<= 8
            outputBitCount -= 8
        End While
    End Sub



    ' This is the expansion routine.  It takes an LZW format file, and expands
    ' it to an bwOutput file.  The code here should be a fairly close match to
    ' the algorithm in the accompanying article.

    Public Sub Expand()
        Dim baDecodeStack(TableSize) As Byte
        Dim iNextCode As Integer
        Dim iNewCode As Integer
        Dim iOldCode As Integer
        Dim bCharacter As Byte
        Dim iCurrCode As Integer
        Dim i As Integer

        'This is the next available code to define.
        iNextCode = 256

        ' Read in the first code, initialize the 
        ' character variable, and send the first 
        ' code to the output file.
        iOldCode = input_code()
        bCharacter = CType(iOldCode, Byte)
        WriteByte(CByte(iOldCode))

        ' This is the main expansion loop.  It reads in characters from the LZW file
        ' until it sees the special code used to inidicate the end of the data.
        iNewCode = input_code()
        While (iNewCode <> _maxValue)
            If iNewCode >= iNextCode Then
                ' This code checks for the special STRING+CHARACTER+STRING+CHARACTER+STRING
                ' case which generates an undefined code.  It handles it by decoding
                ' the last code, and adding a single character to the end of the decode string.            
                baDecodeStack(0) = bCharacter
                i = 1
                iCurrCode = iOldCode
            Else
                ' Otherwise we do a straight decode of the new code.
                i = 0
                iCurrCode = iNewCode
            End If
            While iCurrCode > 255
                ' This routine simply decodes a string from the string table, storing
                ' it in a buffer.  The buffer can then be output in reverse order by
                ' the expansion program.
                baDecodeStack(i) = _baAppendCharacter(iCurrCode)
                i = i + 1
                If i >= _maxCode Then
                    Throw New ApplicationException("Fatal error during iCurrCode expansion.")
                End If
                iCurrCode = _iaPrefixCode(iCurrCode)
            End While
            baDecodeStack(i) = CType(iCurrCode, Byte)
            bCharacter = baDecodeStack(i)

            'Now we output the decoded string in reverse order.
            While i >= 0
                WriteByte(baDecodeStack(i))
                i = i - 1
            End While

            ' Finally, if possible, add a new code to the string table.
            If (iNextCode <= _maxCode) Then
                _iaPrefixCode(iNextCode) = iOldCode
                _baAppendCharacter(iNextCode) = bCharacter
                iNextCode += 1
            End If
            iOldCode = iNewCode
            iNewCode = input_code()
        End While
    End Sub

    ' The following routine is used to input variable length
    ' codes.  It is written strictly for clarity, and is not
    ' particularly efficient.
    Private Function input_code() As Integer
        Dim returnValue As Long
        Static inputBitCount As Integer = 0
        Static inputBitBuffer As Long = 0
        Static mask32 As Long = CLng(2 ^ 32) - 1

        While inputBitCount <= 24
            inputBitBuffer = (inputBitBuffer Or
                ReadByte() << (24 - inputBitCount)) And mask32
            inputBitCount += 8
        End While
        returnValue = (inputBitBuffer >> 32 - _bits) And mask32
        inputBitBuffer = (inputBitBuffer << _bits) And mask32
        inputBitCount -= _bits
        Return CInt(returnValue)
    End Function

    Private Sub WriteByte(ByVal b As Byte)
        BwOutput.Write(b)
    End Sub

    Private Function ReadByte() As Integer
        Dim ba(1) As Byte
        Dim iResult As Integer
        iResult = BrInput.Read(ba, 0, 1)
        If iResult = 0 Then
            Return -1
        End If
        Return ba(0)
    End Function

End Class
