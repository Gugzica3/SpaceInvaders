# Tutorial: Como Montar o Space Invaders na Unity

Eu criei todos os scripts base do jogo necessários para você dentro da pasta `Assets/Scripts`. Agora, você precisa montar a cena na Unity para que eles funcionem. Siga este passo a passo:

## 1. Configurando a Cena e a Câmera
1. Abra sua Main Camera.
2. Mude a projeção dela para **Orthographic**.
3. Ajuste o tamanho (Size) da câmera para algo em torno de `5` a `7`, dependendo do visual que você deseja.
4. Mude a cor de fundo (Background Color) para Preto sólido.

## 2. Configurando o Jogador (Player)
1. Crie um objeto 2D na cena (clique com botão direito na Hierarchy > **2D Object** > **Sprites** > **Square** ou use o seu sprite).
2. Nomeie-o como `Player`.
3. Adicione o componente **Rigidbody2D** ao `Player`:
   - Mude o Body Type para **Kinematic** (para que ele não caia pela gravidade, ou deixe Dynamic com Gravity Scale = 0 e Freeze Rotation Z).
4. Adicione um **BoxCollider2D** e marque a opção **Is Trigger**.
5. Arraste o script `Player.cs` para este objeto.
6. Dentro do script no Inspector:
   - Configure a `Speed` (ex: 5).
   - Defina as `Lives` como 3.
7. Crie um objeto vazio chamado `FirePoint` como "filho" (child) do Player, e mova-o levemente para cima da nave. Arraste este objeto para o campo **Fire Point** no script do Player.

## 3. Configurando os Tiros (Projectiles)
1. Crie um novo Sprite na cena (um pequeno quadrado ou retângulo para representar o tiro do player). Nomeie-o `PlayerProjectile`.
2. Adicione um **BoxCollider2D** (marque Is Trigger) e um **Rigidbody2D** (Gravity Scale = 0).
3. Mude a *Tag* do objeto para `PlayerProjectile` (você terá que criar essa Tag na Unity clicando em Add Tag...).
4. Arraste o script `Projectile.cs` para ele e defina a velocidade (Speed = 10).
5. Arraste este objeto da aba Hierarchy para a pasta `Assets/Prefabs` (crie a pasta se não existir) para transformá-lo num **Prefab**. Depois, delete-o da cena.
6. **Repita o processo** para o tiro do inimigo: nomeie `EnemyProjectile`, crie a Tag `EnemyProjectile`, defina a velocidade dele para `-5` (pois ele desce), e transforme-o num Prefab em `Assets/Prefabs`.

## 4. Configurando os Inimigos (Enemy)
1. Crie um Sprite na cena e nomeie `Enemy`.
2. Mude a Tag dele para `Enemy`.
3. Adicione um **Rigidbody2D** (Gravity Scale = 0, Freeze Rotation Z) e um **BoxCollider2D** (Is Trigger).
4. Arraste o script `Enemy.cs` para ele.
5. No script do Enemy:
   - Speed = 2 (positiva, fará ele começar andando para a direita).
   - Wait Time = 1 (tempo até ele virar a direção).
   - Arraste o prefab do `EnemyProjectile` para o campo Projectile Prefab.
6. Arraste o inimigo para `Assets/Prefabs` e depois coloque vários deles pela cena.

## 5. Configurando a Nave Mãe (Mothership)
1. Crie um novo Sprite chamado `Mothership`. Tag `Enemy`.
2. Adicione Rigidbody2D (0 gravidade), BoxCollider2D (Is Trigger).
3. Adicione o script `Mothership.cs`.
4. Transforme em Prefab e delete da cena.

## 6. Configurando a Interface de Usuário (UI) e GameManager
1. Clique na Hierarchy > UI > **Canvas**.
2. No Canvas, adicione dois textos (UI > Text - TextMeshPro ou UI > Legacy > Text):
   - Um para a pontuação (Score Text).
   - Um para as vidas (Lives Text).
3. Crie dois **Panels** (Painéis) vazios (UI > Panel) que vão cobrir a tela inteira.
   - Nomeie um como `VictoryPanel` e adicione um Texto grande "Você Venceu!" e um botão de Reiniciar.
   - Nomeie o outro como `DefeatPanel` e adicione um Texto "Game Over" e um botão de Reiniciar.
   *(Deixe os dois painéis desativados/invisíveis)*.
4. Crie um objeto vazio (Create Empty) chamado `GameManager`.
5. Arraste os scripts `GameManager.cs` e `UIManager.cs` para ele.
6. **GameManager**:
   - Arraste o prefab da `Mothership`.
   - Crie dois objetos vazios fora da tela (um na esquerda lá no topo e outro na direita). Arraste-os para o campo Spawn Points da Mothership.
7. **UIManager**:
   - Arraste o texto do Score, texto da Vida, VictoryPanel e DefeatPanel para os respectivos campos.
8. Para o *Botão de Reiniciar*, no evento OnClick(), clique no `+`, arraste o `GameManager`, e selecione a função `GameManager.RestartGame`.

## Pronto!
- Dê Play e veja se tudo funciona (Player tira vidas, a pontuação sobe, os inimigos se movem).
- Para a Animação Básica, você pode selecionar o Player/Enemy, abrir a janela de "Animation" (`Window -> Animation -> Animation`), clicar em *Create*, e animar a escala ou os sprites como preferir! Quando você tiver os sprites, basta trocar as imagens nos componentes Sprite Renderer e arrumar as colisões e tags!
