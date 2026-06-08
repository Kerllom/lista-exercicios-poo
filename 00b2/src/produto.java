

/**
 * Representa um produto no estoque.
 * Relacionamento: cada Produto pertence a uma Categoria (N:1).
 * A chave estrangeira categoria_id é mapeada pelo campo categoriaId.
 */
public class produto {

    private int       id;
    private String    nome;
    private String    descricao;
    private double    preco;
    private int       quantidade;
    private int       categoriaId;      // chave estrangeira -> categorias.id
    private categoria categoria;        // objeto associado (carregado via JOIN)

    // ---- Construtores ----

    public produto() {}

    /** Usado para INSERT (sem id, sem objeto Categoria completo). */
    public produto(String nome, String descricao, double preco,
                   int quantidade, int categoriaId) {
        this.nome        = nome;
        this.descricao   = descricao;
        this.preco       = preco;
        this.quantidade  = quantidade;
        this.categoriaId = categoriaId;
    }

    // ---- Getters & Setters ----

    public int getId()                      { return id; }
    public void setId(int id)               { this.id = id; }

    public String getNome()                 { return nome; }
    public void setNome(String nome)        { this.nome = nome; }

    public String getDescricao()                { return descricao; }
    public void setDescricao(String descricao)  { this.descricao = descricao; }

    public double getPreco()                { return preco; }
    public void setPreco(double preco)      { this.preco = preco; }

    public int getQuantidade()                  { return quantidade; }
    public void setQuantidade(int quantidade)   { this.quantidade = quantidade; }

    public int getCategoriaId()                     { return categoriaId; }
    public void setCategoriaId(int categoriaId)     { this.categoriaId = categoriaId; }

    public categoria getCategoria()                 { return categoria; }
    public void setCategoria(categoria categoria)   { this.categoria = categoria; }

    // ---- toString ----

    @Override
    public String toString() {
        String nomeCategoria = (categoria != null) ? categoria.getNome()
                : "ID=" + categoriaId;
        return String.format(
                "Produto[id=%d, nome='%s', preco=R$ %.2f, qtd=%d, categoria='%s']",
                id, nome, preco, quantidade, nomeCategoria);
    }
}