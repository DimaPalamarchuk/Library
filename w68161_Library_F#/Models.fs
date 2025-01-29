module Models

    open System

    type Role =
        | Admin
        | User

    type Book = {
        Id: int
        Title: string
        Author: string
    }

    type User = {
        Id: int
        Name: string
        Role: Role
    }

    type Borrowing = {
        BookId: int
        UserId: int
        BorrowDate: DateTime
    }
