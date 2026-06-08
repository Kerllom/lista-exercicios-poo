package exercicio05.dao;

import exercicio05.model.Departamento;
import exercicio05.model.Funcionario;
import exercicio05.util.Conexao;

import java.sql.*;
import java.util.ArrayList;
import java.util.List;

/**
 * DAO responsável pelas operações de Funcionário.
 *
 * PARA O MATEUS:
 *  - Segue o mesmo padrão do ProdutoDAO do Exercício 2.
 *  - O JOIN com departamentos é igual ao JOIN com categorias.
 *  - O campo mediaAvaliacoes é deixado em 0.0 aqui —
 *    o AvaliacaoDAO cuida do cálculo quando necessário.
 */
public class FuncionarioDAO {

    // ------------------------------------------------------------------
    //  CREATE — Cadastrar funcionário
    // ------------------------------------------------------------------

    public void cadastrar(Funcionario funcionario) throws SQLException {
        String sql = """
                INSERT INTO funcionarios (nome, cargo, departamento_id)
                VALUES (?, ?, ?)
                """;

        try (Connection con = Conexao.obter();
             PreparedStatement ps = con.prepareStatement(sql, Statement.RETURN_GENERATED_KEYS)) {

            ps.setString(1, funcionario.getNome());
            ps.setString(2, funcionario.getCargo());
            ps.setInt(3, funcionario.getDepartamentoId());
            ps.executeUpdate();

            try (ResultSet rs = ps.getGeneratedKeys()) {
                if (rs.next()) funcionario.setId(rs.getInt(1));
            }
            System.out.printf("[DAO] Funcionário '%s' cadastrado com id=%d.%n",
                    funcionario.getNome(), funcionario.getId());
        }
    }

    // ------------------------------------------------------------------
    //  READ — Buscar por id (com JOIN no departamento)
    // ------------------------------------------------------------------

    public Funcionario buscarPorId(int id) throws SQLException {
        String sql = """
                SELECT f.id, f.nome, f.cargo, f.departamento_id,
                       d.nome AS depto_nome
                FROM   funcionarios f
                JOIN   departamentos d ON d.id = f.departamento_id
                WHERE  f.id = ?
                """;

        try (Connection con = Conexao.obter();
             PreparedStatement ps = con.prepareStatement(sql)) {

            ps.setInt(1, id);

            try (ResultSet rs = ps.executeQuery()) {
                if (rs.next()) return mapear(rs);
            }
        }
        return null;
    }

    // ------------------------------------------------------------------
    //  READ — Listar funcionários por departamento (com JOIN)
    // ------------------------------------------------------------------

    public List<Funcionario> listarPorDepartamento(int departamentoId) throws SQLException {
        String sql = """
                SELECT f.id, f.nome, f.cargo, f.departamento_id,
                       d.nome AS depto_nome
                FROM   funcionarios f
                JOIN   departamentos d ON d.id = f.departamento_id
                WHERE  f.departamento_id = ?
                ORDER BY f.nome
                """;

        List<Funcionario> lista = new ArrayList<>();

        try (Connection con = Conexao.obter();
             PreparedStatement ps = con.prepareStatement(sql)) {

            ps.setInt(1, departamentoId);

            try (ResultSet rs = ps.executeQuery()) {
                while (rs.next()) lista.add(mapear(rs));
            }
        }
        return lista;
    }

    // ------------------------------------------------------------------
    //  Utilitário — mapear ResultSet → Funcionario
    // ------------------------------------------------------------------

    private Funcionario mapear(ResultSet rs) throws SQLException {
        Funcionario f = new Funcionario();
        f.setId(rs.getInt("id"));
        f.setNome(rs.getString("nome"));
        f.setCargo(rs.getString("cargo"));
        f.setDepartamentoId(rs.getInt("departamento_id"));

        Departamento d = new Departamento(
                rs.getInt("departamento_id"),
                rs.getString("depto_nome")
        );
        f.setDepartamento(d);

        return f;
    }
}