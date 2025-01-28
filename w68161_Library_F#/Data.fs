module Data

    open Models
    open System

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

    let addBook title author =
        let newId = if books |> List.isEmpty then 1 else (books |> List.maxBy (fun b -> b.Id)).Id + 1
        books <- books @ [{ Id = newId; Title = title; Author = author }]
        newId

    let addUser name =
        let newId = if users |> List.isEmpty then 1 else (users |> List.maxBy (fun u -> u.Id)).Id + 1
        users <- users @ [{ Id = newId; Name = name }]
        newId
