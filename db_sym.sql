drop database sym;

create database sym;
use sym;

create table despesas(
	id int primary key auto_increment,
    estado varchar(8) not null,
    nome varchar(150) not null,
    empresa varchar(150),
    categoria varchar(100) not null,    
    valor decimal(12,2) not null,
    data_vencimento date,
    data_insercao datetime DEFAULT CURRENT_TIMESTAMP
);

insert into despesas (estado, nome, empresa, categoria, valor, data_vencimento) VALUES ("Pendente", "Nada", "DAAE", "Alimentação", 500.22 ,"2020-10-27");

select * from despesas;

create table receitas(
	id int primary key auto_increment,    
    descricao varchar(150) not null,    
    categoria varchar(100) not null,    
    valor decimal(12,2) not null,
    data_insercao datetime DEFAULT CURRENT_TIMESTAMP
);

select * from receitas;

insert into receitas (descricao, categoria, valor) VALUES ("aaaaa", "salário", 500.22);

select * from despesas;
select * from despesas 
	where nome like "%nada%" 
	and empresa like "%DAAE%" 
	and categoria like "%men%" 
	and MONTH(data_vencimento) = "10" 
	and YEAR(data_vencimento) = "2020"
    and estado = "Pago";