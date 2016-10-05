module TableTypes

type Row = {
    values : string array
}

type Table = {
    id : string
    columns: string array
    rows : Row array
}
