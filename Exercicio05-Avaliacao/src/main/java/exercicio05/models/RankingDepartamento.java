package exercicio05.models;

import java.math.BigDecimal;

public class RankingDepartamento {
    private String departamento;
    private BigDecimal mediaGeral;
    private int totalAvaliacoes;

    public RankingDepartamento(String departamento, BigDecimal mediaGeral, int totalAvaliacoes) {
        this.departamento = departamento;
        this.mediaGeral = mediaGeral;
        this.totalAvaliacoes = totalAvaliacoes;
    }

    public String getDepartamento() { return departamento; }
    public BigDecimal getMediaGeral() { return mediaGeral; }
    public int getTotalAvaliacoes() { return totalAvaliacoes; }
}