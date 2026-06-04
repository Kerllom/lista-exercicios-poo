import java.time.LocalDate;
import java.time.format.DateTimeFormatter;

public class Cliente {
private String nome;
private String cpf;
private String email;
private String telefone;
 private LocalDate dataCadastro;
private int id;


    public String getNome(){
        return nome;

    }
    public String getCpf(){
        return cpf;
    }
    public String getEmail(){
        return email;
    }
    public String getTelefone(){
        return telefone;
    }
    public LocalDate getDataCadastro(){
        return dataCadastro;
    }
    public int getId(){
        return id;
    }
    public void setNome(String nome){
        this.nome=nome;
    }
    public void  setCpf(String cpf){
        this.cpf=cpf;
    }
    public void  setEmail(String email){
        this.email=email;
    }
    public void setTelefone(String telefone){
        this.telefone=telefone;
    }
    public void setDataCadastro(LocalDate dataCadastro){
        this.dataCadastro = dataCadastro;
    }
    public void setId(int id){
        this.id= id;
    }

    public Cliente(String nome, String cpf, String email, String telefone) {
        this.nome = nome;
        this.cpf = cpf;
        this.email = email;
        this.telefone = telefone;
        this.dataCadastro = LocalDate.now();
    }
    public Cliente(){}



}

