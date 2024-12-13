namespace LibraryManagementSystem

open System.Data.SqlClient

module Database =
    //let connectionString = "Server=db11009.public.databaseasp.net; Database=db11009; User Id=db11009; Password=7Rj=?i2XY-t5; Encrypt=True; TrustServerCertificate=True; MultipleActiveResultSets=True;"
    let connectionString = "Server=.;Database=LibraryManagementSystem;Trusted_Connection=True;";
        
    let addBook (title: string) (author: string) (genre: string) =
        use conn = new SqlConnection(connectionString)
        conn.Open()
        use cmd = new SqlCommand("INSERT INTO Books (Title, Author, Gender, Status) VALUES (@Title, @Author, @Gender, 'Available')", conn)
        cmd.Parameters.AddWithValue("@Title", title) |> ignore
        cmd.Parameters.AddWithValue("@Author", author) |> ignore
        cmd.Parameters.AddWithValue("@Gender", genre) |> ignore
        cmd.ExecuteNonQuery() |> ignore

    let searchBooks (searchTerm: string) =
        use conn = new SqlConnection(connectionString)
        conn.Open()
        use cmd = new SqlCommand("SELECT * FROM Books WHERE Title LIKE @SearchTerm", conn)
        cmd.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%") |> ignore
        use reader = cmd.ExecuteReader()
        [
            while reader.Read() do
                let status = 
                    if reader.["Status"].ToString() = "Available" then Available
                    else Borrowed(reader.GetDateTime(reader.GetOrdinal("BorrowDate")),
                                reader.["Borrower"].ToString())
                yield {
                    Id = reader.GetInt32(reader.GetOrdinal("Id"))
                    Title = reader.["Title"].ToString()
                    Author = reader.["Author"].ToString()
                    Gender = reader.["Gender"].ToString()
                    Status = status
                }
        ]
let borrowBook (bookId: int) (borrower: string) =
        use conn = new SqlConnection(connectionString)
        conn.Open()
        use cmd = new SqlCommand("""
            UPDATE Books 
            SET Status = 'Borrowed', 
                BorrowDate = @BorrowDate, 
                Borrower = @Borrower 
            WHERE Id = @BookId AND Status = 'Available'""", conn)
        cmd.Parameters.AddWithValue("@BookId", bookId) |> ignore
        cmd.Parameters.AddWithValue("@BorrowDate", DateTime.Now) |> ignore
        cmd.Parameters.AddWithValue("@Borrower", borrower) |> ignore
        cmd.ExecuteNonQuery() > 0

    let returnBook (bookId: int) =
        use conn = new SqlConnection(connectionString)
        conn.Open()
        use cmd = new SqlCommand("""
            UPDATE Books 
            SET Status = 'Available', 
                BorrowDate = NULL, 
                Borrower = NULL 
            WHERE Id = @BookId AND Status = 'Borrowed'""", conn)
        cmd.Parameters.AddWithValue("@BookId", bookId) |> ignore
        cmd.ExecuteNonQuery() > 0

    let getAllBooks() =
        use conn = new SqlConnection(connectionString)
        conn.Open()
        use cmd = new SqlCommand("SELECT * FROM Books", conn)
        use reader = cmd.ExecuteReader()
        [
            while reader.Read() do
                let status = 
                    if reader.["Status"].ToString() = "Available" then Available
                    else Borrowed(reader.GetDateTime(reader.GetOrdinal("BorrowDate")),
                                reader.["Borrower"].ToString())
                yield {
                    Id = reader.GetInt32(reader.GetOrdinal("Id"))
                    Title = reader.["Title"].ToString()
                    Author = reader.["Author"].ToString()
                    Gender = reader.["Gender"].ToString()
                    Status = status
                }
        ]


