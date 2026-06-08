package exercicio05.dao;

import exercicio05.models.Avaliacao;

import java.math.BigDecimal;
import java.sql.*;
import exercicio05.models.RankingDepartamento;
import java.util.ArrayList;
import java.util.List;

public class AvaliacaoDAO {

    public void registrar(Avaliacao avaliacao) {
        String sql = "INSERT INTO avaliacoes (funcionario_id, nota, data_avaliacao) " +
                "VALUES (?, ?, ?)";
        try (Connection conn = Conexao.obterConexao();
             PreparedStatement ps = conn.prepareStatement(sql, Statement.RETURN_GENERATED_KEYS)) {

            ps.setInt(1, avaliacao.getFuncionarioId());
            ps.setBigDecimal(2, avaliacao.getNota());
            ps.setDate(3, Date.valueOf(avaliacao.getDataAvaliacao()));
            ps.executeUpdate();

            try (ResultSet chaves = ps.getGeneratedKeys()) {
                if (chaves.next())
                    avaliacao.setId(chaves.getInt(1));
            }
        } catch (SQLException e) {
            if (e.getErrorCode() == 1452)
                throw new RuntimeException("O funcionario informado nao existe.");
            throw new RuntimeException("Erro ao registrar avaliacao: " + e.getMessage());
        }
    }

    public BigDecimal mediaDoFuncionario(int funcionarioId) {
        String sql = "SELECT AVG(nota) AS media FROM avaliacoes WHERE funcionario_id = ?";
        try (Connection conn = Conexao.obterConexao();
             PreparedStatement ps = conn.prepareStatement(sql)) {

            ps.setInt(1, funcionarioId);
            try (ResultSet rs = ps.executeQuery()) {
                if (rs.next())
                    return rs.getBigDecimal("media"); // vem null se nao houver avaliacoes
            }
        } catch (SQLException e) {
            throw new RuntimeException("Erro ao calcular media: " + e.getMessage());
        }
        return null;
    }

    public List<RankingDepartamento> rankingPorDepartamento() {
        List<RankingDepartamento> ranking = new ArrayList<>();
        String sql = "SELECT d.nome AS departamento, " +
                "       AVG(a.nota) AS media_geral, " +
                "       COUNT(a.id) AS total " +
                "FROM departamentos d " +
                "LEFT JOIN funcionarios f ON f.departamento_id = d.id " +
                "LEFT JOIN avaliacoes a ON a.funcionario_id = f.id " +
                "GROUP BY d.id, d.nome " +
                "ORDER BY media_geral DESC";
        try (Connection conn = Conexao.obterConexao();
             PreparedStatement ps = conn.prepareStatement(sql);
             ResultSet rs = ps.executeQuery()) {

            while (rs.next()) {
                ranking.add(new RankingDepartamento(
                        rs.getString("departamento"),
                        rs.getBigDecimal("media_geral"),
                        rs.getInt("total")
                ));
            }
        } catch (SQLException e) {
            throw new RuntimeException("Erro ao gerar ranking: " + e.getMessage());
        }
        return ranking;
    }
}