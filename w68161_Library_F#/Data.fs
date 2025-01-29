module Data

    open Models
    open System

    let mutable books = [
        { Id = 1; Title = "1984"; Author = "George Orwell" }
        { Id = 2; Title = "To Kill a Mockingbird"; Author = "Harper Lee" }
    ]

    let mutable users = [
        { Id = 1; Name = "Alice"; Role = Admin }
        { Id = 2; Name = "Bob"; Role = User }
        { Id = 3; Name = "Admin"; Role = Admin }
        { Id = 4; Name = "User"; Role = User }
    ]

    let mutable borrowings = [
        { BookId = 1; UserId = 2; BorrowDate = DateTime.Now.AddDays(-10.0) }
    ]

    let finePerDay = 1.0
    let borrowPeriod = 7

    let addBook title author =
        let newId = if books |> List.isEmpty then 1 else (books |> List.maxBy (fun b -> b.Id)).Id + 1
        books <- books @ [{ Id = newId; Title = title; Author = author }]
        newId

    let addUser name role =
        let newId = if users |> List.isEmpty then 1 else (users |> List.maxBy (fun u -> u.Id)).Id + 1
        users <- users @ [{ Id = newId; Name = name; Role = role }]
        newId

    let borrowBook userId bookId =
        if books |> List.exists (fun b -> b.Id = bookId) then
            borrowings <- borrowings @ [{ BookId = bookId; UserId = userId; BorrowDate = DateTime.Now }]
            printfn "Book borrowed successfully."
        else
            printfn "Invalid book ID."

    let getUserFines userId =
        borrowings |> List.choose (fun b ->
            if b.UserId = userId then
                let daysBorrowed = (DateTime.Now - b.BorrowDate).Days
                if daysBorrowed > borrowPeriod then
                    let fine = float (daysBorrowed - borrowPeriod) * finePerDay
                    Some (b.BookId, fine)
                else None
            else None)

    let viewBorrowedBooks userId =
        let userBorrowings = borrowings |> List.filter (fun b -> b.UserId = userId)
        if userBorrowings |> List.isEmpty then
            printfn "You have not borrowed any books."
        else
            printfn "Books you have borrowed:"
            userBorrowings |> List.iter (fun borrowing ->
                let book = books |> List.find (fun b -> b.Id = borrowing.BookId)
                printfn "Title: %s, Author: %s, Borrowed on: %O" book.Title book.Author borrowing.BorrowDate)