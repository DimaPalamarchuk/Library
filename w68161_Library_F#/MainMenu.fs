module MainMenu

    open System
    open Models
    open Data

    let viewBooks () =
        printfn "Books:"
        books |> List.iter (fun book -> printfn "Id: %d, Title: %s, Author: %s" book.Id book.Title book.Author)

    let viewUsers () =
        printfn "Users:"
        users |> List.iter (fun user -> printfn "Id: %d, Name: %s" user.Id user.Name)

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
        printfn "5. View All Fines"
        printfn "6. Exit"
        printf "Choose an option: "
        match Console.ReadLine() with
        | "1" -> viewBooks(); mainMenu()
        | "2" -> 
            printf "Enter book title: "
            let title = Console.ReadLine()
            printf "Enter book author: "
            let author = Console.ReadLine()
            addBook title author |> ignore
            printfn "Book added: %s by %s" title author
            mainMenu()
        | "3" -> viewUsers(); mainMenu()
        | "4" -> 
            printf "Enter user name: "
            let name = Console.ReadLine()
            addUser name |> ignore
            printfn "User added: %s" name
            mainMenu()
        | "5" -> viewAllFines(); mainMenu()
        | "6" -> printfn "Exiting..."
        | _ -> printfn "Invalid option. Try again."; mainMenu()
