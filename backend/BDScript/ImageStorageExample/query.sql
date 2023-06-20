use master
go

if exists(select * from sys.databases where name = 'FullExample')
	drop database FullExample
go

create database FullExample
go

use FullExample
go

create table ImageData(
	ID int identity primary key,
	Photo varbinary(MAX) not null
);
go

create table Location(
	ID int identity primary key,
	Nome varchar(60) not null,
	Photo int references ImageData(ID) null
);
go

select * from ImageData
select * from Location