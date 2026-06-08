import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.SQLException;

/**
 * Gerenciador de conexão com o banco de dados MySQL.
 * Utiliza o padrão Singleton para reutilizar a conexão.
 */
public class conexao {

    // -------------------------------------------------------
    //  Configurações — altere conforme seu ambiente
    // -------------------------------------------------------
    private static final String URL      = "jdbc:mysql://localhost:3306/estoque_db?useSSL=false&serverTimezone=America/Sao_Paulo&allowPublicKeyRetrieval=true";
    private static final String USUARIO  = "root";
    private static final String SENHA    = "sua_senha";   // <-- altere aqui
    // -------------------------------------------------------

    private static Connection instancia;

    /** Impede instanciação direta. */
    private conexao() {}

    /**
     * Retorna uma conexão ativa com o banco.
     * Cria uma nova se a atual estiver fechada ou nula.
     */
    public static Connection obter() throws SQLException {
        if (instancia == null || instancia.isClosed()) {
            try {
                Class.forName("com.mysql.cj.jdbc.Driver");
                instancia = DriverManager.getConnection(URL, USUARIO, SENHA);
                System.out.println("[DB] Conexão estabelecida com sucesso.");
            } catch (ClassNotFoundException e) {
                throw new SQLException("Driver MySQL não encontrado. " +
                        "Adicione o mysql-connector-j ao classpath.", e);
            }
        }
        return instancia;
    }

    /** Fecha a conexão se estiver aberta. */
    public static void fechar() {
        if (instancia != null) {
            try {
                if (!instancia.isClosed()) {
                    instancia.close();
                    System.out.println("[DB] Conexão encerrada.");
                }
            } catch (SQLException e) {
                System.err.println("[DB] Erro ao fechar conexão: " + e.getMessage());
            }
        }
    }
}