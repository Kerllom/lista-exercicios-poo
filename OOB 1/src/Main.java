import java.sql.SQLException;
import java.util.Scanner;

public static void main(String[] args) {
    Scanner leia= new Scanner(System.in);
    int opcao;
    clienteDAO dao =new clienteDAO();

do {


    System.out.println("======================");
    System.out.println("[1]. cadastrar cliente.");
    System.out.println("[2]. listar todos. ");
    System.out.println("[3]. buscar por Cpf.");
    System.out.println("[4]. Atualizar email.");
    System.out.println("[5]. Atualizar telefone.");
    System.out.println("[6]. remover cliente.");
    System.out.println("[0]. sair.");
    System.out.println("======================");
    opcao = leia.nextInt();
    try {

        switch (opcao) {
            case 1 -> {
                System.out.println("Digite o nome:");
                String nome = leia.next();

                System.out.println("Digite o CPF:");
                String cpf = leia.next();

                System.out.println("digite seu email:");
                String email = leia.next();

                System.out.println(" digite seu telefone:");
                String telefone = leia.next();
                Cliente cliente = new Cliente(nome, cpf, email, telefone);
                dao.cadastrar(cliente);
            }

            case 2 -> {
                List<Cliente> lista = dao.listarTodos();
                for (Cliente c : lista) {
                    System.out.println(c.getNome() + " - " + c.getCpf());
                }

            }

            case 3 -> {
                System.out.println(" digite seu cpf:");
                String cpf = leia.next();
                Cliente encontrado = dao.buscarPorCpf(cpf);
                if (encontrado != null) {
                    System.out.println(encontrado.getNome() + " - " + encontrado.getEmail());
                } else {
                    System.out.println("Cliente não encontrado.");
                }
            }

            case 4 -> {
                System.out.println("digite seu cpf:");
                String cpf = leia.next();
                System.out.println("digite o seu novo email:");
                String email = leia.next();
                dao.atualizarEmail(cpf, email);
                System.out.println("Email atualizado com sucesso!");


            }

            case 5 -> {
                System.out.println("digite seu cpf:");
                String cpf = leia.next();
                System.out.println("digite o novo telefone:");
                String telefone = leia.next();
                dao.atualizartelefone(cpf, telefone);
                System.out.println(" telefone atualizado com sucesso! ");
            }

            case 6 -> {
                System.out.println("digite o seu cpf");
                String cpf = leia.next();
                dao.remover(cpf);
            }

            case 0 -> {
                System.out.println("ate logo ");
                return;
            }

            default -> {
                System.out.println("opção invalida !!");
                return;
            }



        }
    }catch(SQLException e){
        System.out.println("Erro: " + e.getMessage());
    }


    } while (opcao != 0) ;


}