// Learn more about F# at http://fsharp.net
// See the 'F# Tutorial' project for more help.
open GameOfLife.FSharp
open System

[<EntryPoint>]
let main argv = 

    let gen = 20
    let rows = 11
    let columns = 11
    
    let liveCells = [|(0,5);(1,5);(2,5);(3,5);(4,5);(5,5);(6,5);(7,5);(8,5);(9,5);(10,5);(5,0);(5,1);(5,2);(5,3);(5,4);(5,6);(5,7);(5,8);(5,9);(5,10)|] 
    let initWorld = GameOfLife.initialize rows columns
    let mutable world = GameOfLife.wakeUpCell liveCells initWorld

    let printCells grid numGen =
        let maxX = Array2D.length1 grid - 1
        let maxY = Array2D.length2 grid - 1
        printfn "Generation : %d" numGen
        for x in [0..maxX] do
            for y in [0..maxY] do
                match grid.[x, y] with
                    | true -> printf "X "
                    | _    -> printf "- "
            printfn ""
        printfn ""
    
    printCells world 0

    for i in [1..gen] do
        Console.ReadLine() |> ignore
        let nextGen = GameOfLife.processNextGen world
        world <- nextGen
        printCells world i        

    Console.ReadLine() |> ignore
    
    0 // return an integer exit code
