import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.util.ArrayList;
import java.util.List;

public class clienteDAO {

    public void cadastrar(Cliente cliente) throws SQLException {
        String sql = " insert into clientes(nome,cpf,email,telefone, data_cadastro) Values(?,?,?,?,?)";

        Connection con = DBconnection.getConnection();
        PreparedStatement ps = con.prepareStatement(sql);
        ps.setString(1, cliente.getNome());
        ps.setString(2, cliente.getCpf());
        ps.setString(3, cliente.getEmail());
        ps.setString(4, cliente.getTelefone());
        ps.setDate(5, java.sql.Date.valueOf(cliente.getDataCadastro()));
        ps.executeUpdate();
    }

        public List<Cliente> listarTodos() throws SQLException{
        List<Cliente> lista= new ArrayList<>();
        String sql = "select * from clientes";

        Connection con = DBconnection.getConnection();
        PreparedStatement ps = con.prepareStatement(sql);
            ResultSet rs = ps.executeQuery();
            while (rs.next()){
                Cliente c = new Cliente();
                c.setNome(rs.getString("nome"));
                c.setCpf(rs.getString("cpf"));
                c.setEmail(rs.getString("email"));
                c.setTelefone(rs.getString("telefone"));
                c.setDataCadastro(rs.getDate("data_cadastro").toLocalDate());
                lista.add(c);



            }
            return lista;



        }
    public Cliente buscarPorCpf(String novoCpf) throws SQLException {
        String sql = "select * from Clientes where cpf = ?";
        Connection con =DBconnection.getConnection();
        PreparedStatement ps = con.prepareStatement(sql);
        ps.setString(1,novoCpf);
        ResultSet rs = ps.executeQuery();

        if ( rs.next()){
            Cliente c = new Cliente();
            c.setNome(rs.getString("nome"));
            c.setCpf(rs.getString("cpf"));
            c.setEmail(rs.getString("email"));
            c.setTelefone(rs.getString("telefone"));
            c.setDataCadastro(rs.getDate("data_cadastro").toLocalDate());
            return c;

        }
        return null;


    }


    public void atualizarEmail(String novoCpf, String novoEmail) throws SQLException {
        String sql= "Update clientes set email = ? where cpf = ?";
        Connection con = DBconnection.getConnection();
        PreparedStatement ps = con.prepareStatement(sql);
        ps.setString(1,novoEmail);
        ps.setString(2,novoCpf);
         ps.executeUpdate();


    }

    public void atualizartelefone(String novotelefone, String novoCpf) throws SQLException{
        String sql = "Update clientes set telefone = ?  where cpf = ?";
        Connection con = DBconnection.getConnection();
        PreparedStatement ps = con.prepareStatement(sql);
        ps.setString(1,novotelefone);
        ps.setString(2,novoCpf);
        ps.executeUpdate();
    }

    public void remover ( String cpf ) throws  SQLException{
        String sql = " Delete from clientes where cpf = ? ";
        Connection con = DBconnection.getConnection();
        PreparedStatement ps = con.prepareStatement(sql);

        ps.setString(1,cpf);
        ps.executeUpdate();


    }











}
