module Models

    open System

    type Book = {
        Id: int
        Title: string
        Author: string
    }

    type User = {
        Id: int
        Name: string
    }

    type Borrowing = {
        BookId: int
        UserId: int
        BorrowDate: DateTime
    }
