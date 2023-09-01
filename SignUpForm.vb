Imports MySql.Data.MySqlClient

Public Class SignUpForm
    Private Sub BTNCreateAccount_Click(sender As Object, e As EventArgs) Handles btnCreateAccount.Click
        ' Call the function to handle user registration
        RegisterUser()
    End Sub

    Private Sub RegisterUser()
        ' MySQL database connection information
        Dim connectionString As String = "Data Source=localhost;Database=fundadatabase;User=root;Password=;"

        Try
            Using connection As New MySqlConnection(connectionString)
                connection.Open()

                ' Prepare the INSERT query
                Dim query As String = "INSERT INTO users (username, password, address, phone) VALUES (@username, @password, @address, @phone)"

                ' Create the command and add parameters
                Using cmd As New MySqlCommand(query, connection)
                    cmd.Parameters.AddWithValue("@username", tbUsername.Text.Trim())
                    cmd.Parameters.AddWithValue("@password", tbPassword.Text.Trim())
                    cmd.Parameters.AddWithValue("@address", tbAddress.Text.Trim())
                    cmd.Parameters.AddWithValue("@phone", tbPhone.Text.Trim())

                    ' Execute the query
                    Dim affectedRows As Integer = cmd.ExecuteNonQuery()

                    If affectedRows > 0 Then
                        MessageBox.Show("User registration successful!")
                    Else
                        MessageBox.Show("User registration failed!")
                    End If
                End Using
            End Using
        Catch ex As MySqlException
            MessageBox.Show("MySQL Error: " & ex.Message)
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub
End Class
