module TableTypes

type Row = {
    values : string array
}

type Table = {
    id : string Option
    columns: string array
    rows : Row array
}
