import sqlite3
from datetime import date

# Connect to the SQLite database (creates the file if it doesn’t exist)
conn = sqlite3.connect('kwin4rh.db')
cursor = conn.cursor()

# Create the employee table
cursor.execute('''
CREATE TABLE IF NOT EXISTS employee (
	FolderNum      INTEGER NOT NULL,
	FolderNumYear  INTEGER NOT NULL,
    Fname TEXT,
    Lname TEXT,
    FnameFr TEXT,
    LnameFr TEXT,
    FatherName TEXT,
    MotherName TEXT,
    Birth DATE,
    Wilaya TEXT,
    Sex BOOLEAN,
    Address TEXT,
    Phone TEXT,
    Email TEXT,
    Relation TEXT,
    HusbandName TEXT,
    ActDate DATE,
    ActNum INTEGER,
	PRIMARY KEY (FolderNum, FolderNumYear)
)
''')

# Commit changes and close connection
conn.commit()






employees = [
    (1001, 2023, "Ali", "Benamara", "Ali", "Benamara", "Omar", "Fatma", "1990-04-12", "Algiers", 1, "12 Rue Didouche Mourad", "0550123456", "Single", "", "2010-06-15", 4512),
    (1002, 2023, "Sofia", "Khelifi", "Sofia", "Khelifi", "Abdallah", "Leila", "1992-09-21", "Oran", 0, "45 Rue Emir Abdelkader", "0550789654", "Married", "Karim", "2013-04-22", 4513),
    (1003, 2023, "Karim", "Bouzar", "Karim", "Bouzar", "Ahmed", "Nora", "1988-01-03", "Constantine", 1, "7 Rue Benbadis", "0660543987", "Single", "", "2009-02-10", 4514),
    (1004, 2023, "Nadia", "Tlemcani", "Nadia", "Tlemcani", "Said", "Hafida", "1991-07-19", "Tlemcen", 0, "10 Avenue Krim Belkacem", "0798342765", "Married", "Rachid", "2011-08-17", 4515),
    (1005, 2023, "Yacine", "Meziane", "Yacine", "Meziane", "Mohamed", "Zohra", "1995-11-08", "Setif", 1, "32 Rue Boudjemaa", "0678023456", "Single", "", "2015-05-09", 4516),
    (1006, 2023, "Samira", "Dali", "Samira", "Dali", "Amar", "Nadia", "1990-12-25", "Bejaia", 0, "23 Rue Ahmed Zabana", "0771234567", "Married", "Mourad", "2012-03-30", 4517),
    (1007, 2023, "Walid", "Cherif", "Walid", "Cherif", "Fouad", "Khadija", "1987-06-14", "Blida", 1, "14 Rue Mokrani", "0567123987", "Single", "", "2008-09-11", 4518),
    (1008, 2023, "Amina", "Zerrouki", "Amina", "Zerrouki", "Hakim", "Salima", "1993-10-28", "Batna", 0, "99 Rue Khemisti", "0550223344", "Married", "Youssef", "2014-07-05", 4519),
    (1009, 2023, "Rami", "Mansouri", "Rami", "Mansouri", "Tahar", "Meriem", "1996-03-02", "Annaba", 1, "5 Rue Baya Hocine", "0798456210", "Single", "", "2016-10-20", 4520),
    (1010, 2023, "Houda", "Boualem", "Houda", "Boualem", "Hassan", "Latifa", "1994-08-11", "Tizi Ouzou", 0, "20 Rue Amirouche", "0678994411", "Married", "Nabil", "2013-12-25", 4521),
]

cursor.executemany('''
INSERT INTO employee (
    FolderNum, FolderNumYear, Fname, Lname, FnameFr, LnameFr, FatherName, MotherName,
    Birth, Wilaya, Sex, Address, Phone, Relation, HusbandName, ActDate, ActNum
) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)
''', employees)
conn.commit()
conn.close()

print("Database and table 'employee' created successfully.")
