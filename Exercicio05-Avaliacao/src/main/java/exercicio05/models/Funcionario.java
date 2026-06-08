package exercicio05.models;

public class Funcionario {
    private int          id;
    private String       nome;
    private String       cargo;
    private int          departamentoId;       // FK → departamentos.id
    private Departamento departamento;         // carregado via JOIN
    private double       mediaAvaliacoes;      // calculado pelo DAO, não vem de coluna

    // ---- Construtores ----

    public Funcionario() {}

    /** Usado para INSERT. */
    public Funcionario(String nome, String cargo, int departamentoId) {
        this.nome            = nome;
        this.cargo           = cargo;
        this.departamentoId  = departamentoId;
    }

    // ---- Getters & Setters ----

    public int getId()                              { return id; }
    public void setId(int id)                       { this.id = id; }

    public String getNome()                         { return nome; }
    public void setNome(String nome)                { this.nome = nome; }

    public String getCargo()                        { return cargo; }
    public void setCargo(String cargo)              { this.cargo = cargo; }

    public int getDepartamentoId()                  { return departamentoId; }
    public void setDepartamentoId(int did)          { this.departamentoId = did; }

    public Departamento getDepartamento()           { return departamento; }
    public void setDepartamento(Departamento d)     { this.departamento = d; }

    public double getMediaAvaliacoes()              { return mediaAvaliacoes; }
    public void setMediaAvaliacoes(double media)    { this.mediaAvaliacoes = media; }

    @Override
    public String toString() {
        String depto = (departamento != null) ? departamento.getNome()
                : "ID=" + departamentoId;
        return String.format(
                "Funcionario[id=%d, nome='%s', cargo='%s', depto='%s', média=%.2f]",
                id, nome, cargo, depto, mediaAvaliacoes);
}
}
