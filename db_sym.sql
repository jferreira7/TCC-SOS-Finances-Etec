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

INSERT INTO saldos (valor, id_usuario) VALUES ("0", "1");
SELECT * FROM saldos;
select valor from saldos where id=1;

drop table objetivos;
select * from objetivos;
INSERT INTO objetivos (nome, preco, imagem, porcentagem, valor_guardado, valor_restante, id_usuario) SELECT nome, preco, imagem, porcentagem, valor_guardado, valor_restante, id_usuario FROM objetivos WHERE id = 1;
create table objetivos (
	id int primary key auto_increment,
    nome varchar(150) not null,
    preco decimal(12,2) not null,
    imagem mediumblob default null,    
    porcentagem decimal(5, 2) default 0,
    valor_guardado decimal(12,2) default 0.00,
    valor_restante decimal(12,2) default 0.00,
    data_insercao datetime DEFAULT CURRENT_TIMESTAMP,
    data_finalizacao datetime,
    id_usuario int not null,
    FOREIGN KEY (id_usuario) REFERENCES usuarios(id)
);

drop trigger objetivos_BEFORE_UPDATE;

DELIMITER //
CREATE TRIGGER objetivos_BEFORE_UPDATE BEFORE UPDATE ON objetivos FOR EACH ROW
BEGIN	
	IF (NEW.valor_guardado >= 0) THEN
		SET NEW.porcentagem = (NEW.valor_guardado * 100)/OLD.preco;   
        SET NEW.valor_restante = OLD.preco -  NEW.valor_guardado;
        UPDATE valores SET valor_saldo = valor_saldo - (NEW.valor_guardado - OLD.valor_guardado), valor_reserva = valor_reserva + (NEW.valor_guardado - OLD.valor_guardado) WHERE id_usuario=NEW.id_usuario; 
	END IF;
	IF (NEW.preco <> OLD.preco) THEN
		SET NEW.porcentagem = (OLD.valor_guardado * 100)/NEW.preco;
        SET NEW.valor_restante = NEW.preco - OLD.valor_guardado;
    END IF;
END;//
DELIMITER ;

update objetivos set valor_guardado = 780 where id = 3 and id_usuario = 1;
select * from objetivos;
select * from valores;
update objetivos set valor_guardado=680 where id=3;
update objetivos set preco=1000 where id=3;
update objetivos set nome="Novo" where id=3;

drop table valores;
create table valores (
	id int primary key auto_increment,    
    valor_saldo decimal(12,2) not null,
    valor_reserva decimal(12,2) not null,
    valor_poupanca decimal(12,2) not null,
    id_usuario int not null,
    FOREIGN KEY (id_usuario) REFERENCES usuarios(id)
);
drop trigger usuarios_AFTER_INSERT;
drop trigger despesas_AFTER_INSERT;
drop trigger receitas_AFTER_INSERT;

drop trigger objetivos_AFTER_INSERT;

DELIMITER //

CREATE TRIGGER usuarios_AFTER_INSERT AFTER INSERT ON usuarios FOR EACH ROW
BEGIN
	INSERT INTO valores (valor_saldo, valor_reserva, valor_poupanca, id_usuario) VALUES ("0", "0", "0", NEW.id);
END;//

CREATE TRIGGER despesas_AFTER_INSERT AFTER INSERT ON despesas FOR EACH ROW
BEGIN
	UPDATE valores set valor_saldo = valor_saldo - new.valor where id_usuario = new.id_usuario;
END;//

CREATE TRIGGER receitas_AFTER_INSERT AFTER INSERT ON receitas FOR EACH ROW
BEGIN
	UPDATE valores set valor_saldo = valor_saldo + new.valor where id_usuario = new.id_usuario;
END;//

CREATE TRIGGER objetivos_AFTER_INSERT AFTER INSERT ON objetivos FOR EACH ROW
BEGIN
	UPDATE valores set valor_saldo = valor_saldo - new.valor_guardado, valor_reserva = valor_reserva + new.valor_guardado where id_usuario = new.id_usuario;
END;//

CREATE TRIGGER objetivos_BEFORE_DELETE BEFORE DELETE ON objetivos FOR EACH ROW
BEGIN
	UPDATE valores set valor_saldo = valor_saldo + OLD.valor_guardado, valor_reserva = valor_reserva - OLD.valor_guardado where id_usuario = OLD.id_usuario;
END;//

DELIMITER ;

SELECT * From valores;
UPDATE valores set valor_saldo = (valor_saldo + 200), valor_reserva = (valor_reserva) - 200 where id_usuario = 1;

select * from valores;
insert into valores (valor_saldo, valor_reserva, valor_poupanca, id_usuario) values (536.54, 268.25, 1520.50 , 1);

-- Trigger poupança







