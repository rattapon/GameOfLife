namespace GameOfLife.FSharp

    module GameOfLife = 

        let initialize role column = 
            let grid = Array2D.init role column (fun _ _ -> false) 
            0







        [<CompiledName("Initialize")>]
        let init role column = initialize role column
