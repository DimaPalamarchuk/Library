module MainMenu

    open System
    open Models
    open Data

    let rec login () =
        printf "Enter your name: "
        let name = Console.ReadLine()
        match users |> List.tryFind (fun u -> u.Name = name) with
        | Some user -> user
        | None -> printfn "User not found."; login()

    let viewBooks () =
        printfn "Books:"
        books |> List.iter (fun book -> printfn "Id: %d, Title: %s, Author: %s" book.Id book.Title book.Author)

    let viewUsers () =
        printfn "Users:"
        users |> List.iter (fun user -> printfn "Id: %d, Name: %s, Role: %A" user.Id user.Name user.Role)

    let viewAllFines () =
        printfn "Fines for overdue books:"
        borrowings |> List.iter (fun borrowing ->
            let daysBorrowed = (DateTime.Now - borrowing.BorrowDate).Days
            if daysBorrowed > borrowPeriod then
                let fine = float (daysBorrowed - borrowPeriod) * finePerDay
                let user = users |> List.find (fun u -> u.Id = borrowing.UserId)
                let book = books |> List.find (fun b -> b.Id = borrowing.BookId)
                printfn "User: %s, Book: %s, Fine: %.2f units." user.Name book.Title fine)

    let viewUserFines userId =
        let fines = getUserFines userId
        if fines |> List.isEmpty then
            printfn "You have no fines."
        else
            fines |> List.iter (fun (bookId, fine) ->
                let book = books |> List.find (fun b -> b.Id = bookId)
                printfn "Book: %s, Fine: %.2f units." book.Title fine)

    let viewMyBorrowedBooks userId =
        viewBorrowedBooks userId

    let rec adminMenu mainMenuRef =
        printfn "\nAdmin Menu:"
        printfn "1. View Books"
        printfn "2. Add Book"
        printfn "3. View Users"
        printfn "4. View All Fines"
        printfn "5. Exit"
        printf "Choose an option: "
        match Console.ReadLine() with
        | "1" -> viewBooks(); adminMenu mainMenuRef
        | "2" ->
            printf "Enter book title: "
            let title = Console.ReadLine()
            printf "Enter book author: "
            let author = Console.ReadLine()
            addBook title author |> ignore
            printfn "Book added: %s by %s" title author
            adminMenu mainMenuRef
        | "3" -> viewUsers(); adminMenu mainMenuRef
        | "4" -> viewAllFines(); adminMenu mainMenuRef
        | "5" -> printfn "Exiting to main menu..."; mainMenuRef()
        | _ -> printfn "Invalid option. Try again."; adminMenu mainMenuRef

    let rec userMenu userId mainMenuRef =
        printfn "\nUser Menu:"
        printfn "1. View Books"
        printfn "2. Borrow Book"
        printfn "3. View My Fines"
        printfn "4. View My Borrowed Books"
        printfn "5. Exit"
        printf "Choose an option: "
        match Console.ReadLine() with
        | "1" -> viewBooks(); userMenu userId mainMenuRef
        | "2" ->
            printf "Enter book ID to borrow: "
            match Console.ReadLine() |> Int32.TryParse with
            | true, bookId -> borrowBook userId bookId
            | _ -> printfn "Invalid book ID."
            userMenu userId mainMenuRef
        | "3" -> viewUserFines userId; userMenu userId mainMenuRef
        | "4" -> viewMyBorrowedBooks userId; userMenu userId mainMenuRef
        | "5" -> printfn "Exiting to main menu..."; mainMenuRef()
        | _ -> printfn "Invalid option. Try again."; userMenu userId mainMenuRef

    let rec mainMenu () =
        printfn "\nMenu:"
        printfn "1. Login"
        printfn "2. Exit"
        printf "Choose an option: "
        match Console.ReadLine() with
        | "1" ->
            let user = login()
            match user.Role with
            | Admin -> adminMenu mainMenu
            | User -> userMenu user.Id mainMenu
        | "2" -> printfn "Exiting program..."; Environment.Exit(0)
        | _ -> printfn "Invalid option. Try again."; mainMenu ()