-- DROP database sym;
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
create table receitas(
	id int primary key auto_increment,    
    descricao varchar(150) not null,    
    categoria varchar(100) not null,    
    valor decimal(12,2) not null,
    data_insercao datetime DEFAULT CURRENT_TIMESTAMP,
    id_usuario int not null,
    FOREIGN KEY (id_usuario) REFERENCES usuarios(id)
);
create table objetivos (
	id int primary key auto_increment,
    estado varchar(10) not null,
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
create table valores (
	id int primary key auto_increment,    
    valor_saldo decimal(12,2) not null,
    valor_reserva decimal(12,2) not null,
    valor_poupanca decimal(12,2) not null,
    id_usuario int not null,
    FOREIGN KEY (id_usuario) REFERENCES usuarios(id)
);
DELIMITER //

	-- OBJETIVOS
    CREATE TRIGGER objetivos_AFTER_INSERT AFTER INSERT ON objetivos FOR EACH ROW
	BEGIN
		UPDATE valores set valor_saldo = valor_saldo - new.valor_guardado, valor_reserva = valor_reserva + new.valor_guardado where id_usuario = new.id_usuario;
	END;//

	CREATE TRIGGER objetivos_BEFORE_DELETE BEFORE DELETE ON objetivos FOR EACH ROW
	BEGIN
		UPDATE valores set valor_saldo = valor_saldo + OLD.valor_guardado, valor_reserva = valor_reserva - OLD.valor_guardado where id_usuario = OLD.id_usuario;
	END;//
	CREATE TRIGGER objetivos_BEFORE_UPDATE BEFORE UPDATE ON objetivos FOR EACH ROW
	BEGIN	
		IF ((NEW.valor_guardado >= 0) AND (NEW.estado not like "Comprado")) THEN
			SET NEW.porcentagem = (NEW.valor_guardado * 100)/OLD.preco;   
			SET NEW.valor_restante = OLD.preco -  NEW.valor_guardado;
			UPDATE valores SET valor_saldo = valor_saldo - (NEW.valor_guardado - OLD.valor_guardado), valor_reserva = valor_reserva + (NEW.valor_guardado - OLD.valor_guardado) WHERE id_usuario=NEW.id_usuario; 
		END IF;
		IF ((NEW.valor_guardado = OLD.preco) AND (NEW.estado not like "Comprado")) THEN
			SET NEW.data_finalizacao = CURRENT_TIMESTAMP;
			SET NEW.estado = "Finalizado";
		END IF;
		IF ((NEW.preco <> OLD.preco) AND (NEW.estado not like "Comprado")) THEN
			SET NEW.porcentagem = (OLD.valor_guardado * 100)/NEW.preco;
			SET NEW.valor_restante = NEW.preco - OLD.valor_guardado;
			
			IF (NEW.preco > OLD.valor_guardado) THEN
				SET NEW.data_finalizacao = NULL;
			ELSE 
				SET NEW.data_finalizacao = CURRENT_TIMESTAMP;
			END IF;
		END IF;
		IF (NEW.estado like "Comprado") THEN		
			INSERT INTO despesas (estado, nome, categoria, valor, data_vencimento, id_usuario) VALUES ("Pago", OLD.nome, "Objetivos", OLD.preco , OLD.data_finalizacao, OLD.id_usuario); 
			UPDATE valores SET valor_reserva = (valor_reserva - OLD.preco) WHERE id_usuario = OLD.id_usuario;
		END IF;
	END;//   
    

	-- DESPESAS
    CREATE TRIGGER despesas_AFTER_INSERT AFTER INSERT ON despesas FOR EACH ROW
	BEGIN		
		IF ((NEW.categoria like "Objetivos") OR (NEW.estado like "Pago")) THEN
			UPDATE valores set valor_saldo = valor_saldo - new.valor where id_usuario = new.id_usuario;
		END IF;        
	END;//
    CREATE TRIGGER despesas_BEFORE_INSERT BEFORE INSERT ON despesas FOR EACH ROW
	BEGIN			
        IF ((NEW.estado like "Pendente") AND (NEW.data_vencimento < CURRENT_DATE())) THEN
			SET NEW.estado = "Atrasado";
        END IF;
	END;//
	CREATE TRIGGER despesas_BEFORE_DELETE BEFORE DELETE ON despesas FOR EACH ROW
	BEGIN
		UPDATE valores set valor_saldo = valor_saldo + OLD.valor where id_usuario = OLD.id_usuario;
	END;//    
	CREATE TRIGGER despesas_BEFORE_UPDATE BEFORE UPDATE ON despesas FOR EACH ROW
	BEGIN
		IF ((NEW.estado like "Pago") AND (OLD.estado not like "Pago")) THEN			
			UPDATE valores set valor_saldo = valor_saldo - NEW.valor where id_usuario = OLD.id_usuario;
		END IF;		
		IF ((NEW.valor <> OLD.valor) AND (NEW.estado like "Pago") AND (OLD.estado like "Pago")) THEN
			UPDATE valores set valor_saldo = valor_saldo - (NEW.valor - OLD.valor) where id_usuario = OLD.id_usuario;			
		END IF;           
        IF ((OLD.estado like "Pago") AND ((NEW.estado like "Pendente") OR (NEW.estado like "Atrasado"))) THEN			
				UPDATE valores set valor_saldo = valor_saldo + OLD.valor where id_usuario = OLD.id_usuario;
		END IF;
        IF ((NEW.estado like "Pendente") AND (NEW.data_vencimento < CURRENT_DATE())) THEN
			SET NEW.estado = "Atrasado";
        END IF;
	END;//
    
    
    
    
    -- RECEITAS
    CREATE TRIGGER receitas_AFTER_INSERT AFTER INSERT ON receitas FOR EACH ROW
	BEGIN
		UPDATE valores set valor_saldo = valor_saldo + new.valor where id_usuario = new.id_usuario;
	END;//      
	CREATE TRIGGER receitas_BEFORE_UPDATE BEFORE UPDATE ON receitas FOR EACH ROW
	BEGIN
		UPDATE valores set valor_saldo = valor_saldo + (NEW.valor - OLD.valor) where id_usuario = OLD.id_usuario;
	END;//
    CREATE TRIGGER receitas_BEFORE_DELETE BEFORE DELETE ON receitas FOR EACH ROW
	BEGIN
		UPDATE valores set valor_saldo = valor_saldo - OLD.valor where id_usuario = OLD.id_usuario;
	END;//   
    
    
    
    -- USUÁRIOS
	CREATE TRIGGER usuarios_AFTER_INSERT AFTER INSERT ON usuarios FOR EACH ROW
	BEGIN
		INSERT INTO valores (valor_saldo, valor_reserva, valor_poupanca, id_usuario) VALUES ("0", "0", "0", NEW.id);
	END;//
    
DELIMITER ;

-- SELECTS
-- select * from objetivos;
-- select * from despesas;
-- select * from receitas;
-- select * from valores;
-- select * from usuarios;

insert into usuarios (nome, email, senha, vencimento_plano) values ("Jean", "jean", "4ff17bc8ee5f240c792b8a41bfa2c58af726d83b925cf696af0c811627714c85", "2020-01-01 10:10:10");





