namespace LibraryManagementSystem

// Domain Types
type BookStatus = 
    | Available 
    | Borrowed of borrowDate:System.DateTime * borrower:string

type Book = {
    Id: int
    Title: string
    Author: string
    Gender: string
    Status: BookStatus
}