import java.sql.SQLException;
import java.util.List;
import java.util.Scanner;

/**
 * Ponto de entrada da aplicação.
 * Menu interativo para demonstrar todas as funcionalidades do sistema.
 */
public class Main {

    private static final categoriadao categoriaDAO = new categoriadao();
    private static final produtoDAO   produtoDAO   = new produtoDAO();
    private static final Scanner      scanner      = new Scanner(System.in);

    public static void main(String[] args) {
        System.out.println("==============================================");
        System.out.println("   SISTEMA DE CONTROLE DE ESTOQUE - MySQL   ");
        System.out.println("==============================================");

        boolean rodando = true;

        while (rodando) {
            exibirMenu();
            int opcao = lerInteiro("Escolha uma opção: ");

            try {
                switch (opcao) {
                    case 1  -> cadastrarCategoria();
                    case 2  -> cadastrarProduto();
                    case 3  -> listarProdutosPorCategoria();
                    case 4  -> listarTodosProdutos();
                    case 5  -> listarTodasCategorias();
                    case 6  -> atualizarEstoque();
                    case 0  -> { rodando = false; System.out.println("\nAté logo!"); }
                    default -> System.out.println("[!] Opção inválida.");
                }
            } catch (SQLException e) {
                System.err.println("[ERRO BD] " + e.getMessage());
            }
        }

        conexao.fechar();
        scanner.close();
    }

    // ------------------------------------------------------------------
    //  Operações de menu
    // ------------------------------------------------------------------

    private static void cadastrarCategoria() throws SQLException {
        System.out.println("\n--- Cadastrar Categoria ---");
        String nome = lerTexto("Nome da categoria: ");
        String desc = lerTexto("Descrição: ");

        categoria cat = new categoria(nome, desc);
        categoriaDAO.cadastrar(cat);
        System.out.println("✔ Categoria cadastrada: " + cat);
    }

    private static void cadastrarProduto() throws SQLException {
        System.out.println("\n--- Cadastrar Produto ---");

        List<categoria> cats = categoriaDAO.listarTodas();
        if (cats.isEmpty()) {
            System.out.println("[!] Nenhuma categoria cadastrada. Cadastre uma categoria primeiro.");
            return;
        }
        System.out.println("Categorias disponíveis:");
        cats.forEach(c -> System.out.printf("  [%d] %s%n", c.getId(), c.getNome()));

        String nome  = lerTexto("Nome do produto: ");
        String desc  = lerTexto("Descrição: ");
        double preco = lerDouble("Preço (ex: 99.90): ");
        int    qtd   = lerInteiro("Quantidade inicial: ");
        int    catId = lerInteiro("ID da categoria: ");

        categoria cat = categoriaDAO.buscarPorId(catId);
        if (cat == null) {
            System.out.println("[!] Categoria não encontrada.");
            return;
        }

        produto p = new produto(nome, desc, preco, qtd, catId);
        produtoDAO.cadastrar(p);
        System.out.println("✔ Produto cadastrado: " + p);
    }

    private static void listarProdutosPorCategoria() throws SQLException {
        System.out.println("\n--- Produtos por Categoria ---");
        int catId = lerInteiro("Informe o ID da categoria: ");

        categoria cat = categoriaDAO.buscarPorId(catId);
        if (cat == null) {
            System.out.println("[!] Categoria não encontrada.");
            return;
        }

        List<produto> produtos = produtoDAO.listarPorCategoria(catId);
        System.out.printf("%nCategoria: %s%n", cat.getNome());
        separador();

        if (produtos.isEmpty()) {
            System.out.println("Nenhum produto cadastrado nesta categoria.");
        } else {
            produtos.forEach(p ->
                    System.out.printf("  ID:%-4d | %-30s | Preço: R$ %8.2f | Estoque: %d%n",
                            p.getId(), p.getNome(), p.getPreco(), p.getQuantidade())
            );
            System.out.printf("Total: %d produto(s)%n", produtos.size());
        }
        separador();
    }

    private static void listarTodosProdutos() throws SQLException {
        System.out.println("\n--- Todos os Produtos ---");
        List<produto> produtos = produtoDAO.listarTodos();
        separador();

        if (produtos.isEmpty()) {
            System.out.println("Nenhum produto cadastrado.");
        } else {
            String catAtual = "";
            for (produto p : produtos) {
                String nomeCat = p.getCategoria().getNome();
                if (!nomeCat.equals(catAtual)) {
                    System.out.printf("%n  [ %s ]%n", nomeCat);
                    catAtual = nomeCat;
                }
                System.out.printf("  ID:%-4d | %-30s | R$ %8.2f | Qtd: %d%n",
                        p.getId(), p.getNome(), p.getPreco(), p.getQuantidade());
            }
            System.out.printf("%nTotal geral: %d produto(s)%n", produtos.size());
        }
        separador();
    }

    private static void listarTodasCategorias() throws SQLException {
        System.out.println("\n--- Categorias Cadastradas ---");
        List<categoria> cats = categoriaDAO.listarTodas();
        separador();

        if (cats.isEmpty()) {
            System.out.println("Nenhuma categoria cadastrada.");
        } else {
            cats.forEach(c ->
                    System.out.printf("  ID:%-4d | %-20s | %s%n",
                            c.getId(), c.getNome(), c.getDescricao())
            );
        }
        separador();
    }

    private static void atualizarEstoque() throws SQLException {
        System.out.println("\n--- Atualizar Estoque ---");
        int prodId  = lerInteiro("ID do produto: ");
        int novaQtd = lerInteiro("Nova quantidade: ");

        boolean atualizado = produtoDAO.atualizarQuantidade(prodId, novaQtd);
        if (atualizado) {
            produto p = produtoDAO.buscarPorId(prodId);
            System.out.println("✔ Estoque atualizado: " + p);
        } else {
            System.out.println("[!] Produto não encontrado.");
        }
    }

    // ------------------------------------------------------------------
    //  Helpers de I/O e formatação
    // ------------------------------------------------------------------

    private static void exibirMenu() {
        System.out.println();
        System.out.println("┌─────────────────────────────────┐");
        System.out.println("│           MENU PRINCIPAL        │");
        System.out.println("├─────────────────────────────────┤");
        System.out.println("│ 1. Cadastrar categoria          │");
        System.out.println("│ 2. Cadastrar produto            │");
        System.out.println("│ 3. Listar produtos por categ.   │");
        System.out.println("│ 4. Listar todos os produtos     │");
        System.out.println("│ 5. Listar todas as categorias   │");
        System.out.println("│ 6. Atualizar estoque            │");
        System.out.println("│ 0. Sair                         │");
        System.out.println("└─────────────────────────────────┘");
    }

    private static void separador() {
        System.out.println("--------------------------------------------------");
    }

    private static String lerTexto(String prompt) {
        System.out.print(prompt);
        return scanner.nextLine().trim();
    }

    private static int lerInteiro(String prompt) {
        while (true) {
            System.out.print(prompt);
            try {
                return Integer.parseInt(scanner.nextLine().trim());
            } catch (NumberFormatException e) {
                System.out.println("[!] Informe um número inteiro válido.");
            }
        }
    }

    private static double lerDouble(String prompt) {
        while (true) {
            System.out.print(prompt);
            try {
                return Double.parseDouble(scanner.nextLine().trim().replace(",", "."));
            } catch (NumberFormatException e) {
                System.out.println("[!] Informe um valor decimal válido.");
            }
        }
    }
}