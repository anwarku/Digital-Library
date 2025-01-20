import pyodbc
from faker import Faker
from datetime import datetime as dt

fake = Faker('id_ID')

SERVER = 'localhost\\SQLEXPRESS01'
DATABASE = 'db_library'
USERNAME = 'TSH\\khaeril.anwar'
PASSWORD = ''
connection_string = f'DRIVER={{ODBC Driver 17 for SQL Server}};SERVER={SERVER};DATABASE={DATABASE};UID={USERNAME};PWD={PASSWORD};Trusted_Connection=yes;'
conn = pyodbc.connect(connection_string)
conn.setdecoding(pyodbc.SQL_CHAR, encoding='latin1')
conn.setencoding('latin1')
cursor = conn.cursor()

# Generate tabel Books
# for i in range(210):
#     count = str(i)
#     sql_query = f"INSERT INTO Books (Code, Title, Author, Publisher, PublishYear, Isbn, Stock) VALUES (?, ?, ?, ?, ?, ?, ?)"
#     data = (
#         f"BK-{fake.date().split('-')[0]}-{count.zfill(4)}", 
#         f"{fake.sentence()}", 
#         f"{fake.name()}", 
#         f"{fake.company()}", 
#         f"{fake.date().split('-')[0]}", 
#         f"{fake.isbn10()}",
#         f"{fake.random_number(2, True)}"
#         )
#     cursor.execute(sql_query, data)
#     conn.commit()

# Generate tabel Members
# for i in range(10):
#     sql_query = f"INSERT INTO Members (Name, Gender, Phone, Job) VALUES (?, ?, ?, ?)"
#     # ID Member adalah AUTO_INCREMENT
#     data = (
#         f"{fake.name()}",
#         f"{fake.random_element(elements=('Male', 'Female'))}",
#         f"{fake.phone_number()}",
#         f"{fake.job()}"
#     )
#     cursor.execute(sql_query, data)
#     conn.commit()

# Debugging tabel
# Mendapatkan sebuah data tabel
# sql_query = "SELECT * FROM Books"
# cursor.execute(sql_query)
# data = cursor.fetchone()
# print(data)

# sql_query = "SELECT * FROM Members"
# cursor.execute(sql_query)
# data = cursor.fetchall()
# print(data)

# sql_query = "SELECT Code FROM Books"
# cursor.execute(sql_query)
# data = cursor.fetchall()
# book_codes = [data[i][0] for i in range(len(data))]
# print(tuple(book_codes))

# Generate tabel Transactions
# for j in range(1, 12):
#     # Mendapatkan tahun sekarang
#     year_now = dt.now().year

#     # Query untuk menambahkan data yang belum mengembalikan
#     sql_query = f"INSERT INTO Transactions (Id, BorrowDate, ReturnDate, MemberId, Status) VALUES (?, ?, ?, ?, ?)"
#     data = (
#         f"{year_now}-{str(j).zfill(4)}",
#         f"{fake.date_between(start_date='-16d', end_date='-8d')}",
#         f"{fake.date_between(start_date='-10d', end_date='-1d')}",
#         f"{fake.random_int(1, 10)}",
#         f"{1}"
#     )
#     cursor.execute(sql_query, data)
#     conn.commit()

# for i in range(12, 21):
#     # Mendapatkan tahun sekarang
#     year_now = dt.now().year

#     # Query untuk menambahkan data yang belum mengembalikan
#     sql_query = f"INSERT INTO Transactions (Id, BorrowDate, MemberId, Status) VALUES (?, ?, ?, ?)"
#     data = (
#         f"{year_now}-{str(i).zfill(4)}",
#         f"{fake.date_between(start_date='-6d', end_date='now')}",
#         f"{fake.random_int(1, 10)}",
#         f"{0}"
#     )
#     cursor.execute(sql_query, data)
#     conn.commit()

# Generate tabel TransactionDetails
# Mendapatkan data transactions
query_transaction = "SELECT Id FROM Transactions"
cursor.execute(query_transaction)
transactions = cursor.fetchall()
transaction_ids = [transactions[i][0] for i in range(len(transactions))]

# # Mendapatkan data books
query_books = "SELECT Code FROM Books"
cursor.execute(query_books)
books = cursor.fetchall()
book_codes = tuple([books[i][0] for i in range(len(books))])

for trans in transaction_ids:
    # Mendapatkan jumlah buku yang dipinjam
    count = fake.random_int(1, 3)
    for i in range(count):
        # Query untuk menambahkan data detail transaksi
        sql_query = f"INSERT INTO DetailTransactions (TransactionId, BookCode) VALUES (?, ?)"
        data = (trans, fake.random_element(elements=book_codes))
        cursor.execute(sql_query, data)
        conn.commit()

conn.close()