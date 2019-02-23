Imports System.IO

Class Form_FroggySpammer
    Public Declare Function GetAsyncKeyState Lib "user32" (ByVal vKey As Integer) As Short
    Public Function TastaApasata(ByVal Tasta As Integer) As Boolean
        Dim Apasata As Short
        Apasata = GetAsyncKeyState(Tasta)
        If Apasata = 0 Then Return False

        Return True
    End Function

    Dim CheckBox_F() As CheckBox
    Dim TextBox_F() As TextBox
    Dim CheckBox_Enter_F() As CheckBox
    Dim Tasta() As Keys

    Private Sub Form_Spammer_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        CheckBox_F = New CheckBox() {CheckBox_F1, CheckBox_F2, CheckBox_F3, CheckBox_F4, CheckBox_F5, CheckBox_F6, CheckBox_F7, CheckBox_F8, CheckBox_F9, CheckBox_F10, CheckBox_F11, CheckBox_F12}
        TextBox_F = New TextBox() {TextBox1, TextBox2, TextBox3, TextBox4, TextBox5, TextBox6, TextBox7, TextBox8, TextBox9, TextBox10, TextBox11, TextBox12}
        CheckBox_Enter_F = New CheckBox() {CheckBox_Enter_F1, CheckBox_Enter_F2, CheckBox_Enter_F3, CheckBox_Enter_F4, CheckBox_Enter_F5, CheckBox_Enter_F6, CheckBox_Enter_F7, CheckBox_Enter_F8, CheckBox_Enter_F9, CheckBox_Enter_F10, CheckBox_Enter_F11, CheckBox_Enter_F12}
        Tasta = New Keys() {Keys.F1, Keys.F2, Keys.F3, Keys.F4, Keys.F5, Keys.F6, Keys.F7, Keys.F8, Keys.F9, Keys.F10, Keys.F11, Keys.F12}

        Timer_Taste.Start()
        Button_Restore.PerformClick()
    End Sub

    Private Sub Timer_Taste_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer_Taste.Tick
        For i = 0 To 11
            If TastaApasata(Tasta(i)) And CheckBox_F(i).Checked Then
                SendKeys.Send(TextBox_F(i).Text)
                If CheckBox_Enter_F(i).Enabled Then
                    SendKeys.Send("{ENTER}")
                End If
            End If
        Next
    End Sub

    Private Sub Button_New_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_New.Click
        For i = 0 To 11
            CheckBox_F(i).Checked = False
            TextBox_F(i).Text = ""
            CheckBox_Enter_F(i).Checked = False
        Next
    End Sub

    Private Sub Button_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Save.Click
        Dim StreamWriter As IO.StreamWriter
        Dim IllegalCharacterFound As Boolean = False
        Dim Phase As String = "delting the old file!"

        Try
            If System.IO.File.Exists("froggy.sav") Then
                Kill("froggy.sav")
            End If

            Phase = "retrieving data from application and filtering/preparing it to be saved!"

            Try
                Dim Line_EnabledKeys As String = "EnabledKeys="
                Dim Line_Texts As String = "Texts="
                Dim Line_IncludesEnter As String = "IncludesEnter="

                For i = 0 To 11
                    Line_EnabledKeys += CheckBox_F(i).Checked & "|"

                    If InStr(TextBox_F(i).Text, "|") > 0 Then
                        IllegalCharacterFound = True
                        TextBox_F(i).Text = TextBox_F(i).Text.Replace("|", "")
                    End If

                    Line_Texts += TextBox_F(i).Text & "|"
                    Line_IncludesEnter += CheckBox_Enter_F(i).Checked & "|"
                Next

                Line_EnabledKeys = Line_EnabledKeys.Substring(0, Line_EnabledKeys.Length - 1)
                Line_Texts = Line_Texts.Substring(0, Line_Texts.Length - 1)
                Line_IncludesEnter = Line_IncludesEnter.Substring(0, Line_IncludesEnter.Length - 1)

                Phase = "accessing froggy.sav!"

                StreamWriter = New IO.StreamWriter("froggy.sav")

                Phase = "writing data to file!"

                StreamWriter.WriteLine(Line_EnabledKeys)
                StreamWriter.WriteLine(Line_Texts)
                StreamWriter.WriteLine(Line_IncludesEnter)

                StreamWriter.Close()
                StreamWriter.Dispose()

                Phase = "clearing data from memory!"

                Line_EnabledKeys = ""
                Line_Texts = ""
                Line_IncludesEnter = ""

                If IllegalCharacterFound Then
                    MsgBox("'|' is an illegal character, all it's instances have been removed!", MsgBoxStyle.Information)
                End If

                MsgBox("Values saved successfully!", MsgBoxStyle.Information, "Success!")
                Button_Restore.PerformClick()
            Catch ErrorSavingFile As Exception
                MsgBox("Error saving 'froggy.sav', while " & Phase, MsgBoxStyle.Critical, "Error!")

                If System.IO.File.Exists("froggy.sav") Then
                    Kill("froggy.sav")
                End If
            End Try
        Catch ErrorSavingFile As Exception
            MsgBox("Error saving 'froggy.sav', while " & Phase, MsgBoxStyle.Critical, "Error!")
            If System.IO.File.Exists("froggy.sav") Then
                Kill("froggy.sav")
            End If
        End Try

        Phase = "locating the file!"

    End Sub

    Private Sub Button_Restore_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Restore.Click
        Dim StreamReader As StreamReader
        Dim Phase As String = "locating the file!"
        Try
            StreamReader = New IO.StreamReader("froggy.sav")

            Dim Result_CheckBox_F As String = ""
            Dim Result_TextBox_F As String = ""
            Dim Result_CheckBox_Enter_F As String = ""

            Phase = "collecting data from file!"

            Do While (StreamReader.Peek <> -1)
                Dim Line As String = StreamReader.ReadLine
                If Line.ToLower.StartsWith("EnabledKeys".ToLower & "=") Then
                    Result_CheckBox_F = Line.Substring("EnabledKeys".Length + 1)
                ElseIf Line.ToLower.StartsWith("Texts".ToLower & "=") Then
                    Result_TextBox_F = Line.Substring("Texts".Length + 1)
                ElseIf Line.ToLower.StartsWith("IncludesEnter".ToLower & "=") Then
                    Result_CheckBox_Enter_F = Line.Substring("IncludesEnter".Length + 1)
                End If
            Loop

            StreamReader.Close()
            StreamReader.Dispose()

            Phase = "applying the saved data!"

            For i = 0 To 11
                CheckBox_F(i).Checked = Result_CheckBox_F.Split("|")(i)
                TextBox_F(i).Text = Result_TextBox_F.Split("|")(i)
                CheckBox_Enter_F(i).Checked = Result_CheckBox_Enter_F.Split("|")(i)
            Next

            Phase = "clearing data from memory!"

            Result_CheckBox_F = ""
            Result_TextBox_F = ""
            Result_CheckBox_Enter_F = ""

            'MsgBox("Values loaded successfully!", MsgBoxStyle.Information, "Success!")
        Catch ErrorOpeningFile As Exception
            Dim MsgBox_StreamReaderError = MsgBox("Error loading 'froggy.sav', while " & Phase & vbNewLine & vbNewLine & "Do you want to save an empty one?", MsgBoxStyle.YesNo, "Error!")

            If MsgBox_StreamReaderError = vbYes Then
                If System.IO.File.Exists("froggy.sav") Then
                    Kill("froggy.sav")
                End If
                Button_New.PerformClick()
                Button_Save.PerformClick()
            Else
                Me.Close()
            End If
        End Try
    End Sub
End Class