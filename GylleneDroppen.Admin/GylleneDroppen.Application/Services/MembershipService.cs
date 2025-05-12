using GylleneDroppen.Application.Common.Results;
using GylleneDroppen.Application.Dtos.Common;
using GylleneDroppen.Application.Dtos.Membership;
using GylleneDroppen.Application.Interfaces.Repositories;
using GylleneDroppen.Application.Interfaces.Services;
using GylleneDroppen.Application.Interfaces.Shared.Repositories;
using GylleneDroppen.Domain.Entities;
using GylleneDroppen.Domain.Enums;

namespace GylleneDroppen.Application.Services;

public class MembershipService(
    IUserRepository userRepository,
    IMembershipRepository membershipRepository,
    IAttendeeRepository attendeeRepository) : IMembershipService
{
    public async Task<Result<MessageResponse>> ActivateFreeTrialAsync(Guid userId)
    {
        var user = await userRepository.GetByIdAsync(userId);
        if (user == null)
            return Result<MessageResponse>.Failure("User not found", 404);

        var existingMembership = await membershipRepository.GetByUserIdAsync(userId);
        if (existingMembership != null)
            return Result<MessageResponse>.Failure("User is already activated", 404);

        var freeTrial = new Membership
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Type = MembershipType.FreeTrial,
            Status = MembershipStatus.Active,
            StartDate = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        await membershipRepository.AddAsync(freeTrial);
        await membershipRepository.SaveChangesAsync();

        return Result<MessageResponse>.Success(new MessageResponse("Free trial successfully activated."));
    }

    public async Task<Result<MessageResponse>> CreateMembershipAsync(Guid userId, CreateMembershipRequest request,
        Guid? adminId = null)
    {
        var user = await userRepository.GetByIdAsync(userId);
        if (user == null)
            return Result<MessageResponse>.Failure("User not found.", 404);

        var existingMembership = await membershipRepository.GetByUserIdAsync(userId);

        if (existingMembership == null)
        {
            // Create new membership
            var membership = new Membership
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Type = request.Type,
                Status = MembershipStatus.Active,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddMonths(request.DurationMonths),
                PaymentReference = request.PaymentReference,
                Notes = request.Notes,
                CreatedAt = DateTime.UtcNow,
                LastUpdatedById = adminId
            };

            await membershipRepository.AddAsync(membership);
        }
        else
        {
            existingMembership.Type = request.Type;
            existingMembership.Status = MembershipStatus.Active;
            existingMembership.StartDate = DateTime.UtcNow;
            existingMembership.EndDate = DateTime.UtcNow.AddMonths(request.DurationMonths);
            existingMembership.PaymentReference = request.PaymentReference;
            existingMembership.Notes = request.Notes;
            existingMembership.UpdatedAt = DateTime.UtcNow;
            existingMembership.LastUpdatedById = adminId;

            membershipRepository.Update(existingMembership);
        }

        await membershipRepository.SaveChangesAsync();

        return Result<MessageResponse>.Success(
            new MessageResponse($"{request.Type} membership successfully created."));
    }

    public async Task<Result<MessageResponse>> CancelMembershipAsync(Guid userId)
    {
        var membership = await membershipRepository.GetByUserIdAsync(userId);
        if (membership == null)
            return Result<MessageResponse>.Failure("User not found", 404);

        if (membership.Status != MembershipStatus.Active)
            return Result<MessageResponse>.Failure("Only active memberships can be cancelled.", 400);

        membership.Status = MembershipStatus.Cancelled;
        membership.UpdatedAt = DateTime.UtcNow;

        membershipRepository.Update(membership);
        await membershipRepository.SaveChangesAsync();

        return Result<MessageResponse>.Success(new MessageResponse("Membership successfully cancelled."));
    }

    public async Task<Result<MembershipStatusResponse>> GetMembershipStatusAsync(Guid userId)
    {
        var user = await userRepository.GetByIdAsync(userId);
        if (user == null)
            return Result<MembershipStatusResponse>.Failure("User not found", 404);

        var membership = await membershipRepository.GetByUserIdAsync(userId);


        var hasActiveMembership = membership is { Status: MembershipStatus.Active };
        var canRegisterForTastings = false;
        var hasValidFreeTrial = false;

        if (hasActiveMembership)
        {
            if (membership!.EndDate.HasValue && membership.EndDate.Value < DateTime.UtcNow)
            {
                membership.Status = MembershipStatus.Expired;
                membershipRepository.Update(membership);
                await membershipRepository.SaveChangesAsync();
                hasActiveMembership = false;
            }
            else
            {
                if (membership.Type != MembershipType.FreeTrial)
                {
                    canRegisterForTastings = true;
                }
                else
                {
                    var attendanceCount = await attendeeRepository.CountUserAttendancesAsync(userId);
                    hasValidFreeTrial = attendanceCount == 0;
                    canRegisterForTastings = hasValidFreeTrial;
                }
            }
        }

        var response = new MembershipStatusResponse
        {
            HasActiveMembership = hasActiveMembership,
            CurrentMembershipType = membership?.Type,
            CurrentMembershipStatus = membership?.Status,
            MembershipStartDate = membership?.StartDate,
            MembershipEndDate = membership?.EndDate,
            CanRegisterForTastings = canRegisterForTastings,
            HasValidFreeTrial = hasValidFreeTrial
        };

        return Result<MembershipStatusResponse>.Success(response);
    }

    public async Task<Result<MessageResponse>> UpdateMembershipAsync(Guid userId, UpdateMembershipRequest request,
        Guid? adminId = null)
    {
        var membership = await membershipRepository.GetByUserIdAsync(userId);
        if (membership == null)
            return Result<MessageResponse>.Failure("Membership not found.", 404);

        var hasChanges = false;

        if (request.Status.HasValue && membership.Status != request.Status.Value)
        {
            membership.Status = request.Status.Value;
            hasChanges = true;
        }

        if (request.Notes != null && membership.Notes != request.Notes)
        {
            membership.Notes = request.Notes;
            hasChanges = true;
        }

        if (!hasChanges)
            return Result<MessageResponse>.Success(new MessageResponse("Membership successfully updated."));

        membership.UpdatedAt = DateTime.UtcNow;
        membership.LastUpdatedById = adminId ?? userId;

        membershipRepository.Update(membership);
        await membershipRepository.SaveChangesAsync();

        return Result<MessageResponse>.Success(new MessageResponse("Membership successfully updated."));
    }

    public async Task<Result<bool>> CanUserRegisterForTastingAsync(Guid userId)
    {
        var membershipStatus = await GetMembershipStatusAsync(userId);

        if (!membershipStatus.IsSuccess)
            // Correctly access the StatusCode property from the Result object
            return Result<bool>.Failure(
                membershipStatus.ErrorMessage ?? "Failed to retrieve membership status",
                membershipStatus.StatusCode ?? 500);

        return Result<bool>.Success(membershipStatus.Data is { CanRegisterForTastings: true });
    }
}