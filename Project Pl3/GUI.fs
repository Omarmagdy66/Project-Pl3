namespace LibraryManagementSystem

open System
open System.Windows.Forms
open System.Drawing

module GUI =
    // Custom Controls
    type ModernButton() as this =
        inherit Button()
        do
            this.FlatStyle <- FlatStyle.Flat
            this.FlatAppearance.BorderSize <- 0
            this.BackColor <- Color.FromArgb(0, 122, 204)
            this.ForeColor <- Color.White
            this.Font <- new Font("Segoe UI", 9.0f)
            this.Cursor <- Cursors.Hand
            this.Height <- 35
            this.Width <- 120

    type ModernTextBox() as this =
        inherit TextBox()
        do
            this.BorderStyle <- BorderStyle.None
            this.Font <- new Font("Segoe UI", 10.0f)
            this.BackColor <- Color.FromArgb(245, 245, 245)
            this.Height <- 30
            this.Padding <- new Padding(5)

    // Dialog Forms
    let showAddBookDialog (onBookAdded: unit -> unit) =
        use form = new Form(
            Text = "Add New Book",
            Width = 400,
            Height = 300,
            StartPosition = FormStartPosition.CenterParent,
            FormBorderStyle = FormBorderStyle.FixedDialog,
            MaximizeBox = false,
            MinimizeBox = false)

        let layout = new TableLayoutPanel(
            Dock = DockStyle.Fill,
            RowCount = 5,
            ColumnCount = 2,
            Padding = new Padding(20))
        
        layout.RowStyles.Clear()
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40.0f)) |> ignore
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40.0f)) |> ignore
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40.0f)) |> ignore
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40.0f)) |> ignore
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100.0f)) |> ignore

        let titleBox = new ModernTextBox()
        let authorBox = new ModernTextBox()
        let genreBox = new ModernTextBox()
        let addButton = new ModernButton(Text = "Add Book")
        
        layout.Controls.Add(new Label(Text = "Title:", Font = new Font("Segoe UI", 9.0f), Dock = DockStyle.Fill), 0, 0)
        layout.Controls.Add(titleBox, 1, 0)
        layout.Controls.Add(new Label(Text = "Author:", Font = new Font("Segoe UI", 9.0f), Dock = DockStyle.Fill), 0, 1)
        layout.Controls.Add(authorBox, 1, 1)
        layout.Controls.Add(new Label(Text = "Gender:", Font = new Font("Segoe UI", 9.0f), Dock = DockStyle.Fill), 0, 2)
        layout.Controls.Add(genreBox, 1, 2)
        layout.Controls.Add(addButton, 1, 3)
        
        layout.ColumnStyles.Clear()
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30.0f)) |> ignore
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70.0f)) |> ignore

        form.Controls.Add(layout)
        
        addButton.Click.Add(fun _ ->
            if String.IsNullOrWhiteSpace(titleBox.Text) || 
               String.IsNullOrWhiteSpace(authorBox.Text) || 
               String.IsNullOrWhiteSpace(genreBox.Text) then
                MessageBox.Show("Please fill in all fields", "Validation Error", 
                              MessageBoxButtons.OK, MessageBoxIcon.Warning) |> ignore
            else
                Database.addBook titleBox.Text authorBox.Text genreBox.Text
                onBookAdded()
                form.Close())
        
        form.ShowDialog() |> ignore

    let showBorrowDialog (bookId: int) (onBookBorrowed: unit -> unit) =
        use form = new Form(
            Text = "Borrow Book",
            Width = 350,
            Height = 200,
            StartPosition = FormStartPosition.CenterParent,
            FormBorderStyle = FormBorderStyle.FixedDialog,
            MaximizeBox = false,
            MinimizeBox = false)

        let layout = new TableLayoutPanel(
            Dock = DockStyle.Fill,
            RowCount = 3,
            ColumnCount = 2,
            Padding = new Padding(20))

        let borrowerBox = new ModernTextBox()
        let borrowButton = new ModernButton(Text = "Borrow")
        
        layout.Controls.Add(new Label(Text = "Borrower:", Font = new Font("Segoe UI", 9.0f)), 0, 0)
        layout.Controls.Add(borrowerBox, 1, 0)
        layout.Controls.Add(borrowButton, 1, 1)
        
        form.Controls.Add(layout)
        
        borrowButton.Click.Add(fun _ ->
            if String.IsNullOrWhiteSpace(borrowerBox.Text) then
                MessageBox.Show("Please enter borrower name", "Validation Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning) |> ignore
            else if Database.borrowBook bookId borrowerBox.Text then
                MessageBox.Show("Book borrowed successfully", "Success",
                              MessageBoxButtons.OK, MessageBoxIcon.Information) |> ignore
                onBookBorrowed()
                form.Close()
            else
                MessageBox.Show("Failed to borrow book. It might already be borrowed.",
                              "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) |> ignore)
        
        form.ShowDialog() |> ignore

    // Main Form
    type MainForm() as this =
        inherit Form()

        let mainTableLayout = new TableLayoutPanel(
            Dock = DockStyle.Fill,
            RowCount = 3,
            ColumnCount = 1,
            Padding = new Padding(20))
        
        let searchPanel = new TableLayoutPanel(
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            Height = 50,
            RowCount = 1)

        let buttonPanel = new FlowLayoutPanel(
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight,
            Padding = new Padding(10),
            AutoSize = true)

        let searchBox = new ModernTextBox(
            Width = 300,
            Dock = DockStyle.Fill,
            Margin = new Padding(10),
            PlaceholderText = "Search for books...")

        let searchButton = new ModernButton(
            Text = "Search",
            Dock = DockStyle.Fill,
            Margin = new Padding(10))
            
        let booksList = new DataGridView(
            Dock = DockStyle.Fill,
            BackgroundColor = Color.White,
            BorderStyle = BorderStyle.None,
            RowHeadersVisible = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            ReadOnly = true,
            Margin = new Padding(10))
        
        let addBookButton = new ModernButton(
            Text = "Add New Book",
            Margin = new Padding(5))

        let borrowButton = new ModernButton(
            Text = "Borrow Book",
            Margin = new Padding(5))

        let returnButton = new ModernButton(
            Text = "Return Book",
            Margin = new Padding(5))
        
        let refreshBooksList() =
            booksList.Rows.Clear()
            Database.getAllBooks()
            |> List.iter (fun book ->
                let status = 
                    match book.Status with
                    | Available -> "Available"
                    | Borrowed(date, borrower) -> sprintf "Borrowed by %s on %s" borrower (date.ToShortDateString())
                let row = [| 
                    box book.Id
                    box book.Title
                    box book.Author
                    box book.Gender
                    box status
                |]
                booksList.Rows.Add(row) |> ignore)

        do
            this.Text <- "Library Management System"
            this.Size <- new Size(800, 600)
            this.StartPosition <- FormStartPosition.CenterScreen
            this.Font <- new Font("Segoe UI", 9.0f)
            
            booksList.Columns.Add("Id", "ID") |> ignore
            booksList.Columns.Add("Title", "Title") |> ignore
            booksList.Columns.Add("Author", "Author") |> ignore
            booksList.Columns.Add("Gender", "Gender") |> ignore
            booksList.Columns.Add("Status", "Status") |> ignore
            
            searchPanel.Controls.Add(searchBox, 0, 0)
            searchPanel.Controls.Add(searchButton, 1, 0)
            searchPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 80.0f)) |> ignore
            searchPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20.0f)) |> ignore
            
            buttonPanel.Controls.AddRange([| 
                addBookButton :> Control
                borrowButton :> Control
                returnButton :> Control
            |])
            
            mainTableLayout.Controls.Add(searchPanel, 0, 0)
            mainTableLayout.Controls.Add(booksList, 0, 1)
            mainTableLayout.Controls.Add(buttonPanel, 0, 2)
            
            this.Controls.Add(mainTableLayout)
            
            // Wire up events
            searchButton.Click.Add(fun _ ->
                booksList.Rows.Clear()
                Database.searchBooks searchBox.Text
                |> List.iter (fun book ->
                    let status = 
                        match book.Status with
                        | Available -> "Available"
                        | Borrowed(date, borrower) -> 
                            sprintf "Borrowed by %s on %s" borrower (date.ToShortDateString())
                    let row = [| 
                        box book.Id
                        box book.Title
                        box book.Author
                        box book.Gender
                        box status
                    |]
                    booksList.Rows.Add(row) |> ignore))
            
            addBookButton.Click.Add(fun _ -> showAddBookDialog refreshBooksList)
            
            borrowButton.Click.Add(fun _ ->
                if booksList.SelectedRows.Count = 0 then
                    MessageBox.Show("Please select a book to borrow", "Selection Required",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information) |> ignore
                else
                    let selectedRow = booksList.SelectedRows.[0]
                    let bookId = int (selectedRow.Cells.[0].Value.ToString())
                    showBorrowDialog bookId refreshBooksList)
            
            returnButton.Click.Add(fun _ ->
                if booksList.SelectedRows.Count = 0 then
                    MessageBox.Show("Please select a book to return", "Selection Required",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information) |> ignore
                else
                    let selectedRow = booksList.SelectedRows.[0]
                    let bookId = int (selectedRow.Cells.[0].Value.ToString())
                    if Database.returnBook bookId then
                        MessageBox.Show("Book returned successfully", "Success",
                                      MessageBoxButtons.OK, MessageBoxIcon.Information) |> ignore
                        refreshBooksList()
                    else
                        MessageBox.Show("Failed to return book. It might not be borrowed.",
                                      "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) |> ignore)
            
            
            refreshBooksList()