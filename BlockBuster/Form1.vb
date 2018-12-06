Public Class frmPlay
    Public Structure KeyStates
        Public right, left As Boolean
    End Structure

    Private keyState As KeyStates
    Private speedx As Single = 12.0F
    Private speedy As Single = 12.0F
    Private score As Integer = 0
    Private lives As Integer = 3
    Private Const paddleSpeed As Integer = 14

    Private Sub frmPlay_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        Select Case e.KeyCode
            Case Keys.A : keyState.left = True
            Case Keys.D : keyState.right = True
        End Select
    End Sub

    Private Sub frmPlay_KeyUp(sender As Object, e As KeyEventArgs) Handles MyBase.KeyUp
        Select Case e.KeyCode
            Case Keys.A : keyState.left = False
            Case Keys.D : keyState.right = False
        End Select
    End Sub

    Public Sub bounceX()
        speedx *= -1
    End Sub

    Public Sub bounceY()
        speedy *= -1
    End Sub

    Public Sub checkCollisions()
        'check paddle collisions
        If getBounds(ball).IntersectsWith(getBounds(paddle)) Then
            bounceY()
        End If

        'check frm collisions
        If ball.Left < 0 Then
            bounceX()
            ball.Left = 0
        End If
        If ball.Left > Me.Width - ball.Width Then
            bounceX()
            ball.Left = Me.Width - ball.Width
        End If
        If ball.Top < 0 Then
            bounceY()
            ball.Top = 0
        End If
        If ball.Top > Me.Height - ball.Height Then
            bounceY()
            ball.Top = Me.Height - ball.Height
        End If
        doBlockCollisions()
    End Sub

    Public Sub doBlockCollisions()
        For Each ctrl As Control In Me.Controls
            If TypeOf (ctrl) Is PictureBox Then
                If ctrl.Name.Length >= 7 AndAlso ctrl.Name.Substring(0, 7) = "Picture" Then
                    If getBounds(ball).IntersectsWith(getBounds(ctrl)) And ctrl.Visible = True Then
                        ctrl.Visible = False
                        score += 1
                        'bounceX()
                        bounceY()
                    End If
                End If
            End If
        Next
    End Sub

    Public Sub updatePaddle()
        If keyState.left Then
            paddle.Left -= paddleSpeed
        End If
        If keyState.right Then
            paddle.Left += paddleSpeed
        End If
        If paddle.Left < 0 Then paddle.Left = 0
        If paddle.Left > Me.Width - paddle.Width Then paddle.Left = Me.Width - paddle.Width
    End Sub

    Public Sub updateLabels()
        lblScore.Text = "Score: " + score.ToString
        lblLives.Text = "Lives: " + lives.ToString
    End Sub

    Private Sub tmrUpdate_Tick(sender As Object, e As EventArgs) Handles tmrUpdate.Tick
        'update collisions
        checkCollisions()
        updatePaddle()
        'increase speed
        speedx += 0.01F
        speedy += 0.01F
        'move ball
        ball.Left += CInt(Int(speedx))
        ball.Top += CInt(Int(speedy))
        'updatelables
        updateLabels()
        'check for win or loss
        checkWin()
    End Sub

    Public Sub checkWin()
        If score = 42 Then
            MsgBox("You Win!")
            score = 0
            score = 0
        End If
        If lives = 0 Then
            MsgBox("You Lose!")
            lives = -1
        End If
    End Sub

    Public Function getBounds(pb As PictureBox) As Rectangle
        Return New Rectangle(pb.Left, pb.Top, pb.Width, pb.Height)
    End Function

    Private Sub frmPlay_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class