package exercicio05;

import exercicio05.dao.AvaliacaoDAO;
import exercicio05.models.Avaliacao;
import exercicio05.models.RankingDepartamento;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.time.LocalDate;
import java.time.format.DateTimeFormatter;
import java.util.List;
import java.util.Scanner;

public class Main {
    private static final AvaliacaoDAO avaliacaoDao = new AvaliacaoDAO();
    private static final Scanner sc = new Scanner(System.in);
    private static final DateTimeFormatter FMT = DateTimeFormatter.ofPattern("dd/MM/yyyy");

    public static void main(String[] args) {
        boolean executando = true;
        while (executando) {
            System.out.println("\n===== AVALIACAO DE FUNCIONARIOS =====");
            System.out.println("1 - Registrar avaliacao");
            System.out.println("2 - Ver media de um funcionario");
            System.out.println("3 - Ranking de departamentos");
            System.out.println("0 - Sair");
            System.out.print("Escolha uma opcao: ");

            String opcao = sc.nextLine();
            try {
                switch (opcao) {
                    case "1": registrarAvaliacao(); break;
                    case "2": verMedia(); break;
                    case "3": verRanking(); break;
                    case "0": executando = false; break;
                    default: System.out.println("Opcao invalida.");
                }
            } catch (Exception e) {
                System.out.println("\n[ERRO] " + e.getMessage());
            }
        }
        System.out.println("Encerrando o sistema...");
    }

    private static void registrarAvaliacao() {
        System.out.print("ID do funcionario: ");
        int funcionarioId = Integer.parseInt(sc.nextLine());
        System.out.print("Nota (0 a 10, ex: 8.5): ");
        BigDecimal nota = new BigDecimal(sc.nextLine());
        System.out.print("Data (dd/MM/yyyy): ");
        LocalDate data = LocalDate.parse(sc.nextLine(), FMT);

        Avaliacao avaliacao = new Avaliacao();
        avaliacao.setFuncionarioId(funcionarioId);
        avaliacao.setNota(nota);
        avaliacao.setDataAvaliacao(data);

        avaliacaoDao.registrar(avaliacao);
        System.out.println("Avaliacao registrada! ID: " + avaliacao.getId());
    }

    private static void verMedia() {
        System.out.print("ID do funcionario: ");
        int funcionarioId = Integer.parseInt(sc.nextLine());
        BigDecimal media = avaliacaoDao.mediaDoFuncionario(funcionarioId);
        if (media == null)
            System.out.println("Este funcionario ainda nao possui avaliacoes.");
        else
            System.out.println("Media do funcionario: " + media.setScale(2, RoundingMode.HALF_UP));
    }

    private static void verRanking() {
        List<RankingDepartamento> ranking = avaliacaoDao.rankingPorDepartamento();
        if (ranking.isEmpty()) {
            System.out.println("Nenhum departamento cadastrado.");
            return;
        }
        System.out.println("\n--- RANKING DE DEPARTAMENTOS (por media) ---");
        int posicao = 1;
        for (RankingDepartamento r : ranking) {
            String media = (r.getMediaGeral() == null)
                    ? "sem avaliacoes"
                    : r.getMediaGeral().setScale(2, RoundingMode.HALF_UP).toString();
            System.out.println(posicao + "o) " + r.getDepartamento() +
                    " | Media: " + media +
                    " | Avaliacoes: " + r.getTotalAvaliacoes());
            posicao++;
        }
    }
}