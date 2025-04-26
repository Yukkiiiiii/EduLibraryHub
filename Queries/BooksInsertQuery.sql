BULK INSERT dbo.Book
  FROM 'C:\Users\PC\OneDrive\Desktop\Библиотека Ема 21-1 CSV.csv' --Change Based on csv location
  WITH
  (
    FORMAT      = 'CSV',
    FIELDQUOTE  = '"',
    FIRSTROW    = 2,
    CODEPAGE    = '1251'
  );
