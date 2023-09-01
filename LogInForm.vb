Imports MySql.Data.MySqlClient
Imports ADODB
Public Class LogInForm
    Dim connection As New Connection
    Public conn As New MySqlConnection("Data Source=localhost;Database=fundadatabase;User=root;Password=;")

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'ConnectToDatabase()
        Try
            conn.Open()
        Catch ex As Exception
            MessageBox.Show("Error connecting to MySQL database: " & ex.Message)
        Finally
            conn.Close()
        End Try
    End Sub

    Private Sub ConnectToDatabase()
        Dim connectionString As String = "Provider=MySQLProv;Data Source=localhost;User Id=username;Password=password;Database=your_database;"
        connection.ConnectionString = connectionString

        Try
            connection.Open()
            MessageBox.Show("Connected to the database.")
        Catch ex As Exception
            MessageBox.Show("Connection failed: " & ex.Message)
        End Try
    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If connection.State = ConnectionState.Open Then
            connection.Close()
        End If
        ' ... Initialize and bind the productDataTable to DataGridView1 (same as in the previous answer) ...
    End Sub

    Private Sub BTNLogin_Click(sender As Object, e As EventArgs) Handles BTNLogIn.Click
        LoginUser()
    End Sub

    Private Sub LoginUser()
        ' MySQL database connection information
        Dim connectionString As String = "Data Source=localhost;Database=fundadatabase;User=root;Password=;"

        Try
            Using connection As New MySqlConnection(connectionString)
                connection.Open()

                ' Prepare the SELECT query
                Dim query As String = "SELECT COUNT(*) FROM users WHERE username = @username AND password = @password"

                ' Create the command and add parameters
                Using cmd As New MySqlCommand(query, connection)
                    cmd.Parameters.AddWithValue("@username", TXTEmail.Text.Trim())
                    cmd.Parameters.AddWithValue("@password", TXTPassword.Text.Trim())

                    ' Execute the query and get the result (number of matching rows)
                    Dim result As Integer = Convert.ToInt32(cmd.ExecuteScalar())

                    If result > 0 Then
                        Dim MainForm As New MainForm()
                        MainForm.Show()
                        Me.Hide()
                        'Dim MainForm As New MAIN()
                        'MainForm.Show()
                        'Me.Hide()
                    Else
                        MessageBox.Show("wrong username or password")
                    End If
                End Using
            End Using
        Catch ex As MySqlException
            MessageBox.Show("MySQL Error: " & ex.Message)
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub

    Private Sub TXTClose_Click(sender As Object, e As EventArgs) Handles BTNClose.Click
        Me.Close() ' This will close the current form
    End Sub

    Private Sub BtnSignUp_Click(sender As Object, e As EventArgs) Handles BTNSignUp.Click
        Dim SignUpForm As New SignUpForm()
        SignUpForm.Show()
    End Sub
End Class

'' Replace these hardcoded credentials with your actual authentication logic
'Dim validUsername As String = "gelgin"
'Dim validPassword As String = "password"

'Dim enteredUsername As String = txtEmail.Text
'Dim enteredPassword As String = txtPassword.Text

'If enteredUsername = validUsername AndAlso enteredPassword = validPassword Then
'    Dim form2 As New Form2()
'    form2.Show()
'    Me.Hide() ' Hide Form1 (optional, you can close it instead if not needed)
'Else
'    MessageBox.Show("Invalid username or password. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
'End If
'End Sub

'Dim server As String = "10.4.28"
'Dim database As String = "fundadatabse"
'Dim username As String = "root"
'Dim password As String = ""

'Dim connectionString As String = $"server={server};user={username};password={password};database={database};"
'Dim connection As New MySqlConnection(connectionString)

'Try
'    conn.Open()
'    MessageBox.Show("Connection successful!")
'Catch ex As Exception
'    MessageBox.Show("Error connecting to MySQL database: " & ex.Message)
'Finally
'    conn.Close()
'End Try
