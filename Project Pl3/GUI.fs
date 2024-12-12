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