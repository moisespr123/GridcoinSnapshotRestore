Imports System.IO

Public Class Form1
    Private Sub RestoreSnapshot()
        'We will send a signal to properly close the Gridcoin Wallet
        Dim GridcoinProcess As Process() = Process.GetProcessesByName("gridcoinresearch")
        For Each GRCProcess In GridcoinProcess
            GRCProcess.CloseMainWindow()
            'While loop to wait for the process to exit
            While GRCProcess.HasExited = False
                'Waits 1 second, then it will check again if the process has exited
                Threading.Thread.Sleep(1000)
            End While
            'As of this moment, the Gridcoin Wallet should has been closed properly
        Next
        'Now, we will get the AppData Path
        Dim AppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\GridcoinResearch"
        'Deletes the chainstate, database and txleveldb folders
        If My.Computer.FileSystem.DirectoryExists(AppDataPath & "\chainstate") Then My.Computer.FileSystem.DeleteDirectory(AppDataPath & "\chainstate", FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.DeletePermanently)
        If My.Computer.FileSystem.DirectoryExists(AppDataPath & "\database") Then My.Computer.FileSystem.DeleteDirectory(AppDataPath & "\database", FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.DeletePermanently)
        If My.Computer.FileSystem.DirectoryExists(AppDataPath & "\txleveldb") Then My.Computer.FileSystem.DeleteDirectory(AppDataPath & "\txleveldb", FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.DeletePermanently)
        'Deletes the blk0001.dat file if it exists
        If My.Computer.FileSystem.FileExists(AppDataPath & "\blk0001.dat") Then My.Computer.FileSystem.DeleteFile(AppDataPath & "\blk0001.dat", FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.DeletePermanently)
        'Runs the snapshot extraction
        Dim ExtractProcess As New ProcessStartInfo("C:\Program Files\7-zip\7z.exe")
        ExtractProcess.Arguments = "x """ & TextBox1.Text & """ -o""" & AppDataPath & """"
        Dim StartProcess As Process = Process.Start(ExtractProcess)
        StartProcess.WaitForExit()
        'Snapshot should now be restored and we will launch the wallet again!
        If My.Computer.FileSystem.FileExists("C:\Program Files (x86)\GridcoinResearch\gridcoinresearch.exe") Then
            Process.Start("C:\Program Files (x86)\GridcoinResearch\gridcoinresearch.exe")
        ElseIf My.Computer.FileSystem.FileExists("C:\Program Files\GridcoinResearch\gridcoinresearch.exe") Then
            Process.Start("C:\Program Files\GridcoinResearch\gridcoinresearch.exe")
        Else
            MsgBox("GridcoinResearch.exe could not be found. Please launch the wallet manually")
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        OpenFileDialog1.Title = "Browse for a snapshot file"
        OpenFileDialog1.FileName = Path.GetFileName(TextBox1.Text)
        OpenFileDialog1.Filter = "Archive (*.zip;*.7z)|*.zip;*.7z"
        OpenFileDialog1.ShowDialog()
        If String.IsNullOrEmpty(OpenFileDialog1.FileName) = False Then TextBox1.Text = OpenFileDialog1.FileName
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        RestoreSnapshot()
    End Sub
End Class
