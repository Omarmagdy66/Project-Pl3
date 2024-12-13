namespace LibraryManagementSystem

open System
open System.Data.SqlClient

module Database =
    //let connectionString = "Server=db11009.public.databaseasp.net; Database=db11009; User Id=db11009; Password=7Rj=?i2XY-t5; Encrypt=True; TrustServerCertificate=True; MultipleActiveResultSets=True;"
    let connectionString = "Server=.;Database=LibraryManagementSystem;Trusted_Connection=True;";

    let createTables() =
        use conn = new SqlConnection(connectionString)
        conn.Open()
        
    let addBook (title: string) (author: string) (genre: string) =
        use conn = new SqlConnection(connectionString)
        conn.Open()
        use cmd = new SqlCommand("INSERT INTO Books (Title, Author, Gender, Status) VALUES (@Title, @Author, @Gender, 'Available')", conn)
        cmd.Parameters.AddWithValue("@Title", title) |> ignore
        cmd.Parameters.AddWithValue("@Author", author) |> ignore
        cmd.Parameters.AddWithValue("@Gender", genre) |> ignore
        cmd.ExecuteNonQuery() |> ignore
