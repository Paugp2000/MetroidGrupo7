/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Main.java to edit this template
 */
package metroidcomunicationdatabase;

import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.FileWriter;
import java.io.IOException;
import java.io.ObjectOutputStream;
import java.util.Scanner;
import java.sql.SQLException;
import java.util.ArrayList;
import java.sql.*;

public class MetroidComunicationDatabase {

    public static File buscarFitxer(String nomFitx) {
        File fitx = new File ("/MetroidProyecto/MetroidGrupo7/MetroidGrupo7/Proyecto_final/Metroid.txt");
        return fitx;
    }

    public static void main(String[] args) throws ClassNotFoundException{
        Scanner input = new Scanner(System.in);
        Class.forName("com.mysql.cj.jdbc.Driver");
        int contador = 0;
        int rowCount = 0;
        String NombreUsuario [] = new String [10];
        String Tiempo [] = new String [10];
        String NombreUsuarioDB [] = new String [10];
        String TiempoDB [] = new String [10];
        String nomFitx = "Metroid.txt";
        File fitxer = buscarFitxer(nomFitx);
        try (Scanner entrada = new Scanner(fitxer)) {
            while (entrada.hasNext()) {
                NombreUsuario[contador] = entrada.next();
                Tiempo[contador] = entrada.next();
            }
            System.out.printf("%s\n", NombreUsuario[contador]);
            System.out.printf("%s\n", Tiempo[contador]);
            
            
        } catch (FileNotFoundException e) {
            System.out.println("Error: No es pot trobar el fitxer " + nomFitx);
        }

        try 
            (Connection conn = DriverManager.getConnection("jdbc:mysql://dam.inspedralbes.cat/Metroid_Metroid", "Metroid_Admin", "AdminAlReves03.");
            Statement stmt = conn.createStatement();
        )
        {
            PreparedStatement preparedStatement = conn.prepareStatement("insert into Leaderboard(NombreUsuario, Tiempo)" + "values (?,?)");
            preparedStatement.setString(1, NombreUsuario[contador]);
            preparedStatement.setString(2, Tiempo[contador]);
            preparedStatement.executeUpdate();
            
            String strSelect = "select NombreUsuario, Tiempo from Leaderboard";
            ResultSet rset = stmt.executeQuery(strSelect);
            while (rset.next()){
            NombreUsuarioDB[rowCount] = rset.getString("NombreUsuario");
            TiempoDB[rowCount] = rset.getString("Tiempo");
            rowCount ++;
            }
            contador++;
                
        } catch (SQLException ex) {
            System.out.println("Ha aparegut un error, no s'ha trobat la base de dades.\n");
        }
        
        try (ObjectOutputStream output = new ObjectOutputStream(new FileOutputStream("Metroid.dat"))){
            for (int i = 0; i<rowCount; i++){
                output.writeUTF(NombreUsuarioDB[i]);
                output.writeUTF(TiempoDB[i]);
            }
        }catch(IOException ex){
            System.out.printf("Ha aparegut un error.\n");
        }
          
    }
}
