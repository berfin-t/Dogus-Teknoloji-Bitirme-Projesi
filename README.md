# DbBlogApp Projesi

Bu proje, **Doğuş Teknoloji Bootcamp** bitirme projesi olarak geliştirilmiştir. Blog uygulaması, SQL Server veritabanını kullanarak blog verilerini yönetmek için bir çözüm sunmaktadır.

## Başlarken

Projeyi kendi bilgisayarınızda çalıştırabilmek için aşağıdaki adımları izleyin.

### 1. Projeyi İndirin

Projeyi [bu bağlantıdan](https://github.com/berfin-t/Dogus-Teknoloji-Bitirme-Projesi) indirebilir veya GitHub üzerinden klonlayabilirsiniz:

```bash
git clone https://github.com/berfin-t/Dogus-Teknoloji-Bitirme-Projesi.git

### 2. Veritabanını Yükleyin

Projede bulunan `DbBlogApp.bak` yedeğini SQL Server veritabanınıza yüklemeniz gerekmektedir. Bunu yapmak için şu adımları takip edin:

#### SQL Server Management Studio (SSMS) Kullanarak Veritabanını Yükleyin

1. **SQL Server Management Studio (SSMS)**'yi açın.
2. **Object Explorer** panelinde, **Databases** üzerine sağ tıklayın ve **Restore Database...** seçeneğine tıklayın.

   Açılan pencerede:

   - **Source** kısmını **Device** olarak seçin.
   - **Add** butonuna tıklayın ve `.bak` dosyasının bulunduğu dizini seçin.
   - **Destination** kısmına geri yüklemek istediğiniz veritabanı adını yazın.
   - **OK** butonuna tıklayarak işlemi başlatın.

Bu adımların ardından, `.bak` dosyasındaki veritabanı SQL Server veritabanınıza başarıyla geri yüklenmiş olacaktır.

### Projeyi Çalıştırma

Veritabanını başarıyla yükledikten sonra, projeyi açabilir ve çalıştırabilirsiniz. Uygulamanın tüm özellikleri ve fonksiyonları bu veritabanı üzerinden çalışacaktır.

