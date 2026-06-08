/**
 * Representa uma categoria de produtos no estoque.
 * Relacionamento: uma Categoria possui muitos Produtos (1:N).
 */
public class categoria {

    private int    id;
    private String nome;
    private String descricao;

    // ---- Construtores ----

    public categoria() {}

    public categoria(String nome, String descricao) {
        this.nome      = nome;
        this.descricao = descricao;
    }

    public categoria(int id, String nome, String descricao) {
        this.id        = id;
        this.nome      = nome;
        this.descricao = descricao;
    }

    // ---- Getters & Setters ----

    public int getId()                  { return id; }
    public void setId(int id)           { this.id = id; }

    public String getNome()             { return nome; }
    public void setNome(String nome)    { this.nome = nome; }

    public String getDescricao()            { return descricao; }
    public void setDescricao(String desc)   { this.descricao = desc; }

    // ---- toString ----

    @Override
    public String toString() {
        return String.format("Categoria[id=%d, nome='%s', descricao='%s']",
                id, nome, descricao);
    }
}