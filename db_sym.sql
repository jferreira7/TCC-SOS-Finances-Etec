drop database sym;

create database sym;
use sym;

create table usuarios (
	id int primary key auto_increment,
    nome varchar(150) not null,
    email varchar(150) not null unique,
    senha varchar(150) not null,
    vencimento_plano datetime not null,
    created_at datetime default current_timestamp
);
select * from usuarios;

create table despesas(
	id int primary key auto_increment,
    estado varchar(8) not null,
    nome varchar(150) not null,
    empresa varchar(150),
    categoria varchar(100) not null,    
    valor decimal(12,2) not null,
    data_vencimento date,
    data_insercao datetime DEFAULT CURRENT_TIMESTAMP,
    id_usuario int not null,
    FOREIGN KEY (id_usuario) REFERENCES usuarios(id)
);

insert into despesas (estado, nome, empresa, categoria, valor, data_vencimento, id_usuario) VALUES ("Pendente", "Nada", "DAAE", "Alimentação", 500.22 ,"2020-10-27", "1");
select * from despesas;

create table receitas(
	id int primary key auto_increment,    
    descricao varchar(150) not null,    
    categoria varchar(100) not null,    
    valor decimal(12,2) not null,
    data_insercao datetime DEFAULT CURRENT_TIMESTAMP,
    id_usuario int not null,
    FOREIGN KEY (id_usuario) REFERENCES usuarios(id)
);

select * from receitas;
insert into receitas (descricao, categoria, valor, id_usuario) VALUES ("aaaaa", "salário", 756.84, 1);

select * from despesas;
select * from despesas 
	where nome like "%nada%" 
	and empresa like "%DAAE%" 
	and categoria like "%men%" 
	and MONTH(data_vencimento) = "10" 
	and YEAR(data_vencimento) = "2020"
    and estado = "Pago";
    
create table saldos (
	id int primary key auto_increment,
    valor decimal(12,2) not null,
    id_usuario int not null,
    FOREIGN KEY (id_usuario) REFERENCES usuarios(id)
);

DELIMITER //

CREATE TRIGGER usuarios_AFTER_INSERT AFTER INSERT ON usuarios FOR EACH ROW
BEGIN
	INSERT INTO saldos (valor, id_usuario) VALUES ("0", new.id);
END;//

CREATE TRIGGER despesas_AFTER_INSERT AFTER INSERT ON despesas FOR EACH ROW
BEGIN
	UPDATE saldos set valor = valor - new.valor where id_usuario = new.id_usuario;
END;//

CREATE TRIGGER receitas_AFTER_INSERT AFTER INSERT ON receitas FOR EACH ROW
BEGIN
	UPDATE saldos set valor = valor + new.valor where id_usuario = new.id_usuario;
END;//

DELIMITER ;

INSERT INTO saldos (valor, id_usuario) VALUES ("0", "1");
SELECT * FROM saldos;
select valor from saldos where id=1;

drop table objetivos;
create table objetivos (
	id int primary key auto_increment,
    nome varchar(150) not null,
    preco decimal(12,2) not null,
    imagem mediumblob default null,
    valor_mes decimal(12,2) not null,
    valor_inicial decimal(12,2),
    meses_guardados int default 0,
    valor_guardado decimal(12,2) default 0.00,
    valor_restante decimal(12,2) default 0.00,
    data_insercao datetime DEFAULT CURRENT_TIMESTAMP,
    data_finalizacao datetime DEFAULT CURRENT_TIMESTAMP,
    id_usuario int not null,
    FOREIGN KEY (id_usuario) REFERENCES usuarios(id)
);

select * from objetivos;

drop trigger objetivos_AFTER_INSERT;

DELIMITER //

CREATE TRIGGER objetivos_AFTER_INSERT AFTER INSERT ON objetivos FOR EACH ROW
BEGIN
	set new.valor_guardado=new.valor_inicial, new.valor_restante=(new.preco - new.valor_inicial);
END;//

DELIMITER ;

insert into objetivos (nome, preco, valor_mes, valor_inicial, id_usuario) values ("AAAAA", 200.00, 50.00, 100.00, 1);
