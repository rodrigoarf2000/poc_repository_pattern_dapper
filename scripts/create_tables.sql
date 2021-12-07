
CREATE TABLE Author (
    AuthorId int identity(1,1) NOT NULL,
    FullName varchar(200) NOT NULL,
	BirthDate Date NOT NULL,
	CONSTRAINT PK_Author PRIMARY KEY (AuthorId),
);
GO

CREATE TABLE Category (
    CategoryId int identity(1,1) NOT NULL,
    Name varchar(200) NOT NULL,
	Status bit NOT NULL,
	CONSTRAINT PK_Category PRIMARY KEY (CategoryId),
);
GO

CREATE TABLE Book (
    BookId int identity(1,1) NOT NULL,
    AuthorId int NOT NULL,
	CategoryId int NOT NULL,
    Title varchar(200),
	Isbn varchar(200),
	ReleaseDate Date NOT NULL,
	CONSTRAINT PK_Book PRIMARY KEY (BookId),
	CONSTRAINT FK_Book_Author FOREIGN KEY (AuthorId) REFERENCES Author(AuthorId),
	CONSTRAINT FK_Book_Category FOREIGN KEY (CategoryId) REFERENCES Category(CategoryId)
);
GO
