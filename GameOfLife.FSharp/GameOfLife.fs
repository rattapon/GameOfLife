namespace GameOfLife.FSharp

open System
    
    module GameOfLife = 
    
        let initialize role column = 
            if role < 1 || column < 1 then raise (System.ArgumentException("Cannot initialize with size that is smaller than 1"))
            let grid = Array2D.init role column (fun _ _ -> false)
            grid

        let wakeUpCell coordinates (grid:bool[,]) = 
            let mutable newGrid :bool[,] = Array2D.copy grid
            for coordinate in coordinates do
                match coordinate with 
                    | (x, y) -> newGrid.[x, y] <- true
            newGrid         






        [<CompiledName("Initialize")>]
        let init role column = initialize role column

        [<CompiledName("LiveCell")>]
        let wake coordinates grid = wakeUpCell coordinates grid
