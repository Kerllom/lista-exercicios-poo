package exercicio05.models;

import java.math.BigDecimal;
import java.time.LocalDate;

public class Avaliacao {
    private int id;
    private int funcionarioId;
    private BigDecimal nota;
    private LocalDate dataAvaliacao;

    public int getId() { return id; }
    public void setId(int id) { this.id = id; }

    public int getFuncionarioId() { return funcionarioId; }
    public void setFuncionarioId(int funcionarioId) { this.funcionarioId = funcionarioId; }

    public BigDecimal getNota() { return nota; }
    public void setNota(BigDecimal nota) {
        if (nota == null)
            throw new IllegalArgumentException("A nota nao pode ser vazia.");
        if (nota.compareTo(BigDecimal.ZERO) < 0 || nota.compareTo(new BigDecimal("10")) > 0)
            throw new IllegalArgumentException("A nota deve estar entre 0 e 10.");
        this.nota = nota;
    }

    public LocalDate getDataAvaliacao() { return dataAvaliacao; }
    public void setDataAvaliacao(LocalDate dataAvaliacao) {
        if (dataAvaliacao == null)
            throw new IllegalArgumentException("A data da avaliacao e obrigatoria.");
        this.dataAvaliacao = dataAvaliacao;
    }
}