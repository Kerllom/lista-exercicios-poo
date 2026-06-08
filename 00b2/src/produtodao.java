import java.sql.*;
import java.util.ArrayList;
import java.util.List;

/**
 * DAO responsável pelas operações de banco de dados da entidade Produto.
 * Mantém o relacionamento com Categoria através da chave estrangeira categoria_id.
 */
public class produtoDAO {

    // ------------------------------------------------------------------
    //  CREATE — Cadastrar produto vinculado a uma categoria
    // ------------------------------------------------------------------

    public void cadastrar(produto produto) throws SQLException {
        String sql = """
                INSERT INTO produtos (nome, descricao, preco, quantidade, categoria_id)
                VALUES (?, ?, ?, ?, ?)
                """;

        try (Connection con = conexao.obter();
             PreparedStatement ps = con.prepareStatement(sql, Statement.RETURN_GENERATED_KEYS)) {

            ps.setString(1, produto.getNome());
            ps.setString(2, produto.getDescricao());
            ps.setDouble(3, produto.getPreco());
            ps.setInt(4, produto.getQuantidade());
            ps.setInt(5, produto.getCategoriaId());
            ps.executeUpdate();

            try (ResultSet rs = ps.getGeneratedKeys()) {
                if (rs.next()) {
                    produto.setId(rs.getInt(1));
                }
            }
            System.out.printf("[DAO] Produto '%s' cadastrado com id=%d.%n",
                    produto.getNome(), produto.getId());
        }
    }

    // ------------------------------------------------------------------
    //  READ — Buscar por id (com JOIN na categoria)
    // ------------------------------------------------------------------

    public produto buscarPorId(int id) throws SQLException {
        String sql = """
                SELECT p.id, p.nome, p.descricao, p.preco, p.quantidade, p.categoria_id,
                       c.nome AS cat_nome, c.descricao AS cat_desc
                FROM   produtos p
                JOIN   categorias c ON c.id = p.categoria_id
                WHERE  p.id = ?
                """;

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
    //  READ — Listar produtos por categoria
    // ------------------------------------------------------------------

    public List<produto> listarPorCategoria(int categoriaId) throws SQLException {
        String sql = """
                SELECT p.id, p.nome, p.descricao, p.preco, p.quantidade, p.categoria_id,
                       c.nome AS cat_nome, c.descricao AS cat_desc
                FROM   produtos p
                JOIN   categorias c ON c.id = p.categoria_id
                WHERE  p.categoria_id = ?
                ORDER BY p.nome
                """;

        List<produto> lista = new ArrayList<>();

        try (Connection con = conexao.obter();
             PreparedStatement ps = con.prepareStatement(sql)) {

            ps.setInt(1, categoriaId);

            try (ResultSet rs = ps.executeQuery()) {
                while (rs.next()) {
                    lista.add(mapear(rs));
                }
            }
        }
        return lista;
    }

    // ------------------------------------------------------------------
    //  READ — Listar todos os produtos
    // ------------------------------------------------------------------

    public List<produto> listarTodos() throws SQLException {
        String sql = """
                SELECT p.id, p.nome, p.descricao, p.preco, p.quantidade, p.categoria_id,
                       c.nome AS cat_nome, c.descricao AS cat_desc
                FROM   produtos p
                JOIN   categorias c ON c.id = p.categoria_id
                ORDER BY c.nome, p.nome
                """;

        List<produto> lista = new ArrayList<>();

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
    //  UPDATE — Atualizar quantidade em estoque
    // ------------------------------------------------------------------

    public boolean atualizarQuantidade(int produtoId, int novaQuantidade) throws SQLException {
        if (novaQuantidade < 0) {
            throw new IllegalArgumentException("Quantidade não pode ser negativa.");
        }

        String sql = "UPDATE produtos SET quantidade = ? WHERE id = ?";

        try (Connection con = conexao.obter();
             PreparedStatement ps = con.prepareStatement(sql)) {

            ps.setInt(1, novaQuantidade);
            ps.setInt(2, produtoId);

            int linhasAfetadas = ps.executeUpdate();
            if (linhasAfetadas > 0) {
                System.out.printf("[DAO] Estoque do produto id=%d atualizado para %d unidades.%n",
                        produtoId, novaQuantidade);
            }
            return linhasAfetadas > 0;
        }
    }

    // ------------------------------------------------------------------
    //  Utilidade — mapear ResultSet → Produto (com Categoria embutida)
    // ------------------------------------------------------------------

    private produto mapear(ResultSet rs) throws SQLException {
        produto p = new produto();
        p.setId(rs.getInt("id"));
        p.setNome(rs.getString("nome"));
        p.setDescricao(rs.getString("descricao"));
        p.setPreco(rs.getDouble("preco"));
        p.setQuantidade(rs.getInt("quantidade"));
        p.setCategoriaId(rs.getInt("categoria_id"));

        categoria cat = new categoria(
                rs.getInt("categoria_id"),
                rs.getString("cat_nome"),
                rs.getString("cat_desc")
        );
        p.setCategoria(cat);

        return p;
    }
}