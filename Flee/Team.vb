Public Enum AffinityEnum
    KIND = 2 'Not hostile to other KIND teams
    MEAN = 4 'Hostile to KIND, but not always so other MEAN
    ALOOF = 8 'Alway hostile to other teams
End Enum

Public Class Team
    Public world As World
    Public bot_team As Boolean = True
    Shared last_id As Integer = -1
    Shared available_colors As List(Of Color) = New List(Of Color)
    Public Sub New(world As World, affinity As Integer)
        Me.world = world

        'id
        Me.id = last_id + 1
        last_id = Me.id

        'affinity
        If affinity = 0 Then
            If id = 0 Then
                Me.affinity = AffinityEnum.KIND ' player
            ElseIf id = 1 Then
                Me.affinity = AffinityEnum.MEAN ' derelict
            ElseIf id = 2 Then
                Me.affinity = AffinityEnum.ALOOF ' bosses/endgames
            ElseIf id Mod 8 = 0 Then
                Me.affinity = AffinityEnum.KIND
            Else
                Me.affinity = AffinityEnum.MEAN
            End If
        Else
            Me.affinity = affinity
        End If

        'max ships
        If id <> 0 Then
            MaxShips = 72
        End If

        'allow upgrades of bosses + endgame ships
        If id = 2 Then
            upgrade_limit += 1
        End If

        'color
        If available_colors.Count = 0 Then
            available_colors.Add(Color.FromArgb(0, 255, 0)) ' primary green
            available_colors.Add(Color.FromArgb(128, 255, 128)) ' pale green
            available_colors.Add(Color.FromArgb(173, 136, 26)) ' olive
            available_colors.Add(Color.FromArgb(0, 160, 0)) ' dark green
            available_colors.Add(Color.FromArgb(128, 128, 128)) ' gray
            available_colors.Add(Color.FromArgb(0, 80, 255)) ' deep blue
            available_colors.Add(Color.FromArgb(128, 128, 255)) ' pale blue
            available_colors.Add(Color.FromArgb(0, 255, 255)) ' primary cyan
            available_colors.Add(Color.FromArgb(255, 255, 255)) ' white
            available_colors.Add(Color.FromArgb(173, 76, 38)) ' brown
            available_colors.Add(Color.FromArgb(128, 0, 255)) ' dark purple
            available_colors.Add(Color.FromArgb(255, 0, 255)) ' primary magenta
            available_colors.Add(Color.FromArgb(255, 110, 0)) ' orange
            available_colors.Add(Color.FromArgb(173, 34, 69)) ' crismon
            available_colors.Add(Color.FromArgb(255, 255, 0)) ' primary yellow
            available_colors.Add(Color.FromArgb(255, 128, 128)) ' pale red
            available_colors.Add(Color.FromArgb(255, 0, 0)) ' primary red
        End If
        Dim i_color As Integer
        If (Me.affinity And AffinityEnum.KIND) <> 0 Then
            i_color = world.Rand.Next(0, available_colors.Count / 4)
        Else
            i_color = world.Rand.Next(available_colors.Count / 4, available_colors.Count)
        End If
        Me.color = available_colors(i_color)
        available_colors.RemoveAt(i_color)
    End Sub

    Public id As Integer
    Public affinity As Integer
    Public color As Color

    Public upgrade_limit As Integer = 0
    Public resources As MaterialSet = New MaterialSet()
    Public MaxShips As UShort = 8
    Public upgrade_slots_bonus As UShort = 0
    Public ApproxShipCount = 0

    Public Function IsFriendWith(other As Team) As Boolean
        If Me Is other Then
            Return True
        End If
        If other Is Nothing Then
            Return False
        End If
        If (Me.affinity And AffinityEnum.KIND) <> 0 AndAlso (other.affinity And AffinityEnum.KIND) <> 0 Then
            Return True
        End If
        If (Me.affinity And AffinityEnum.MEAN) <> 0 AndAlso (other.affinity And AffinityEnum.MEAN) <> 0 Then
            If (Me.id Mod 6) = (other.id Mod 6) Then
                Return True
            End If
        End If
        Return False
    End Function

End Class
