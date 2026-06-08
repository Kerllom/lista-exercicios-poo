import java.sql.*;
import java.util.ArrayList;
import java.util.List;

/**
 * DAO (Data Access Object) responsável pelas operações
 * de banco de dados da entidade Categoria.
 */
public class categoriadao {

    // ------------------------------------------------------------------
    //  CREATE — Cadastrar uma nova categoria
    // ------------------------------------------------------------------

    public void cadastrar(categoria categoria) throws SQLException {
        String sql = "INSERT INTO categorias (nome, descricao) VALUES (?, ?)";

        try (Connection con = conexao.obter();
             PreparedStatement ps = con.prepareStatement(sql, Statement.RETURN_GENERATED_KEYS)) {

            ps.setString(1, categoria.getNome());
            ps.setString(2, categoria.getDescricao());
            ps.executeUpdate();

            try (ResultSet rs = ps.getGeneratedKeys()) {
                if (rs.next()) {
                    categoria.setId(rs.getInt(1));
                }
            }
            System.out.printf("[DAO] Categoria '%s' cadastrada com id=%d.%n",
                    categoria.getNome(), categoria.getId());
        }
    }

    // ------------------------------------------------------------------
    //  READ — Buscar por id
    // ------------------------------------------------------------------

    public categoria buscarPorId(int id) throws SQLException {
        String sql = "SELECT id, nome, descricao FROM categorias WHERE id = ?";

        try (Connection con = conexao.obter();
             PreparedStatement ps = con.prepareStatement(sql)) {

            ps.setInt(1, id);

            try (ResultSet rs = ps.executeQuery()) {
                if (rs.next()) {
                    return mapear(rs);
                }
            }
        }
        return null;
    }

    // ------------------------------------------------------------------
    //  READ — Listar todas
    // ------------------------------------------------------------------

    public List<categoria> listarTodas() throws SQLException {
        String sql = "SELECT id, nome, descricao FROM categorias ORDER BY nome";
        List<categoria> lista = new ArrayList<>();

        try (Connection con = conexao.obter();
             PreparedStatement ps = con.prepareStatement(sql);
             ResultSet rs = ps.executeQuery()) {

            while (rs.next()) {
                lista.add(mapear(rs));
            }
        }
        return lista;
    }

    // ------------------------------------------------------------------
    //  Utilidade — mapear ResultSet → Categoria
    // ------------------------------------------------------------------

    private categoria mapear(ResultSet rs) throws SQLException {
        return new categoria(
                rs.getInt("id"),
                rs.getString("nome"),
                rs.getString("descricao")
        );
    }
}