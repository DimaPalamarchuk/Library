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

let mutable books = [
    { Id = 1; Title = "1984"; Author = "George Orwell" }
    { Id = 2; Title = "To Kill a Mockingbird"; Author = "Harper Lee" }
]

let mutable users = [
    { Id = 1; Name = "Alice" }
    { Id = 2; Name = "Bob" }
]

let mutable borrowings = [
    { BookId = 1; UserId = 1; BorrowDate = DateTime.Now.AddDays(-10.0) }
]

let finePerDay = 1.0
let borrowPeriod = 7 

let viewBooks () =
    printfn "Books:"
    books |> List.iter (fun book -> printfn "Id: %d, Title: %s, Author: %s" book.Id book.Title book.Author)

let addBook () =
    printf "Enter book title: "
    let title = Console.ReadLine()
    printf "Enter book author: "
    let author = Console.ReadLine()
    let newId = if books |> List.isEmpty then 1 else (books |> List.maxBy (fun b -> b.Id)).Id + 1
    books <- books @ [{ Id = newId; Title = title; Author = author }]
    printfn "Book added: %s by %s" title author

let addUser () =
    printf "Enter user name: "
    let name = Console.ReadLine()
    let newId = if users |> List.isEmpty then 1 else (users |> List.maxBy (fun u -> u.Id)).Id + 1
    users <- users @ [{ Id = newId; Name = name }]
    printfn "User added: %s" name

let viewUsers () =
    printfn "Users:"
    users |> List.iter (fun user -> printfn "Id: %d, Name: %s" user.Id user.Name)

let viewUsersByBook () =
    printf "Enter book title: "
    let title = Console.ReadLine()
    match books |> List.tryFind (fun b -> b.Title = title) with
    | Some book ->
        let borrowedUsers =
            borrowings
            |> List.filter (fun b -> b.BookId = book.Id)
            |> List.map (fun b -> users |> List.find (fun u -> u.Id = b.UserId))
        if List.isEmpty borrowedUsers then
            printfn "No users have borrowed the book: %s" title
        else
            printfn "Users who borrowed the book '%s':" title
            borrowedUsers |> List.iter (fun user -> printfn "Name: %s" user.Name)
    | None -> printfn "Book not found"

let returnBook () =
    printf "Enter book title: "
    let title = Console.ReadLine()
    printf "Enter user name: "
    let userName = Console.ReadLine()
    match books |> List.tryFind (fun b -> b.Title = title), users |> List.tryFind (fun u -> u.Name = userName) with
    | Some book, Some user ->
        match borrowings |> List.tryFind (fun b -> b.BookId = book.Id && b.UserId = user.Id) with
        | Some borrowing ->
            let daysBorrowed = (DateTime.Now - borrowing.BorrowDate).Days
            let fine = if daysBorrowed > borrowPeriod then float (daysBorrowed - borrowPeriod) * finePerDay else 0.0
            borrowings <- borrowings |> List.filter (fun b -> not (b.BookId = book.Id && b.UserId = user.Id))
            printfn "Book '%s' returned by %s." title userName
            if fine > 0.0 then printfn "Late return! Fine: %.2f units." fine
        | None -> printfn "This user has not borrowed the book '%s'." title
    | _ -> printfn "Book or user not found."

let viewAllFines () =
    printfn "Fines for overdue books:"
    borrowings |> List.iter (fun borrowing ->
        let daysBorrowed = (DateTime.Now - borrowing.BorrowDate).Days
        if daysBorrowed > borrowPeriod then
            let fine = float (daysBorrowed - borrowPeriod) * finePerDay
            let user = users |> List.find (fun u -> u.Id = borrowing.UserId)
            let book = books |> List.find (fun b -> b.Id = borrowing.BookId)
            printfn "User: %s, Book: %s, Fine: %.2f units." user.Name book.Title fine
    )

let rec mainMenu () =
    printfn "\nLibrary Menu:"
    printfn "1. View Books"
    printfn "2. Add Book"
    printfn "3. View Users"
    printfn "4. Add User"
    printfn "5. View Users by Book"
    printfn "6. Return Book"
    printfn "7. View All Fines"
    printfn "8. Exit"
    printf "Choose an option: "
    match Console.ReadLine() with
    | "1" -> viewBooks(); mainMenu()
    | "2" -> addBook(); mainMenu()
    | "3" -> viewUsers(); mainMenu()
    | "4" -> addUser(); mainMenu()
    | "5" -> viewUsersByBook(); mainMenu()
    | "6" -> returnBook(); mainMenu()
    | "7" -> viewAllFines(); mainMenu()
    | "8" -> printfn "Exiting..."
    | _ -> printfn "Invalid option. Try again."; mainMenu()

[<EntryPoint>]
let main argv =
    mainMenu()
    0
