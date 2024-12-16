namespace LibraryManagementSystem

type BookStatus = 
    | Available 
    | Borrowed of borrowDate:System.DateTime * borrower:string

